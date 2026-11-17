using Balls;
using Managers;
using Player.ScriptableObjects;
using UnityEngine;
using Utils;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        public int PlayerId;

        // PROPERTIES
        public PlayerEvents PlayerEvents { get; } = new();
        public BlopType BlopType { get; private set; }
        public Blop Blop => _blop;
        public Rigidbody2D Rigidbody => _rb2d;
        public PlayerCountTouchBall CountTouchBall => _playerCountTouchBall;
        public bool HasTheBall => _hasTheBall;
        public bool IsGrounded => _isGrounded;
        public bool IsDashing => _isDashing;

        [Header("Blop")]
        [SerializeField] private Blop _blop;

        [Header("References")]
        [SerializeField] private Rigidbody2D _rb2d;
        [SerializeField] private Animator _animator;
        [SerializeField] private PlayerCountTouchBall _playerCountTouchBall;
        [SerializeField] private PlayerInputHandler _playerInputHandler;
        [SerializeField] private GameObject _laserTrigger;

        [Header("States")]
        [SerializeField] private bool _hasTheBall;
        [SerializeField] private bool _isGrounded;
        [SerializeField] private bool _isWalledLeft;
        [SerializeField] private bool _isWalledRight;
        [SerializeField] private bool _isDashing;
        [SerializeField] private bool _isAbsorbing;
        [SerializeField] private bool _isPerfectReception;
        [SerializeField] private bool _isSpecialSpike;
        [SerializeField] private bool _isPunchingBall;

        [Header("Permissions")]
        [SerializeField] private bool _canMove = true;
        [SerializeField] private bool _canDoubleJump;

        // ===== CONSTANTES =====
        private const float StickDeadZone = 0.1f;
        private const float PunchAnimationDuration = 0.2f;
        private const int PerfectReceptionsForSpecialSpike = 3;
        private const float WallRayHeightOffset = 0.5f;

        private static readonly int AnimIsGrounded = Animator.StringToHash("IsGrounded");
        private static readonly int AnimIsWalled = Animator.StringToHash("IsWalled");
        private static readonly int AnimIsDashing = Animator.StringToHash("IsDashing");
        private static readonly int AnimIsPerfectReception = Animator.StringToHash("IsPerfectReception");
        private static readonly int AnimIsPunchingBall = Animator.StringToHash("IsPunchingBall");
        private static readonly int AnimIsAbsorbing = Animator.StringToHash("IsAbsorbing");
        private static readonly int AnimHasTheBall = Animator.StringToHash("HasTheBall");
        private static readonly int AnimVelocityX = Animator.StringToHash("VelocityX");
        private static readonly int AnimVelocityY = Animator.StringToHash("VelocityY");
        private static readonly int AnimDrawnBall = Animator.StringToHash("DrawnBall");
        private static readonly int AnimJumping = Animator.StringToHash("Jumping");
        private static readonly int AnimJumpingWall = Animator.StringToHash("JumpingWall");
        private static readonly int AnimWinMatch = Animator.StringToHash("WinMatch");
        private static readonly int AnimLoseMatch = Animator.StringToHash("LoseMatch");

        // MOVEMENT
        private Vector2 _move;

        // DASH
        private float _dashTimeRemaining;
        private float _dashCooldownRemaining;

        // PERFECT RECEPTION
        private int _perfectReceptionCount;
        private float _perfectReceptionTimeRemaining;
        private float _perfectReceptionCooldownRemaining;

        private IBall _ball;

        private bool CanSpecialSpike => _perfectReceptionCount >= PerfectReceptionsForSpecialSpike;
        private bool IsWalled => _isWalledLeft || _isWalledRight;
        private bool HasHorizontalInput => Mathf.Abs(_move.x) >= StickDeadZone;

        #region ===== SETUP =====

        private void Awake()
        {
            // Joueur posé directement dans une scène de test : le Blop du prefab fait foi
            if (_blop) BlopType = _blop.BlopType;
        }

        public void SetUp(Blop blop, int playerId)
        {
            _blop = blop;
            PlayerId = playerId;
            BlopType = blop.BlopType;

            _animator.runtimeAnimatorController = _blop.PlayerAnimatorController;
            Instantiate(_blop.PSTrailRenderer, transform);
        }

        public void Die()
        {
            ReleaseBall();
            gameObject.SetActive(false);
            _move = Vector2.zero;
            _isAbsorbing = false;

            EventBus.OnPlayerDie?.Invoke(PlayerId);
        }

        public void ResetSpecialSpikeState()
        {
            _hasTheBall = false;
            _isSpecialSpike = false;
            _rb2d.constraints = RigidbodyConstraints2D.FreezeRotation;
        }

        #endregion

        #region ===== LOOP =====

        private void Update()
        {
            Raycasts();
            UpdatePerfectReception();

            if (_isGrounded) _isAbsorbing = false;

            // FOUL : toucher le sol en tenant la balle
            if (_hasTheBall && _isGrounded)
            {
                ReleaseBall();
                if (MatchManager.Instance != null)
                {
                    MatchManager.Instance.Foul(PlayerId);
                }
            }

            UpdateAnimatorStates();
        }

        private void FixedUpdate()
        {
            if (_dashCooldownRemaining >= 0)
            {
                _dashCooldownRemaining -= Time.fixedDeltaTime;
            }

            if (_isDashing)
            {
                UpdateDash();
                return;
            }

            if (_canMove && !_hasTheBall)
            {
                _rb2d.velocity = new Vector2(_move.x * (_blop.Speed * Time.fixedDeltaTime), _rb2d.velocity.y);
            }

            FlipSprite(_move.x);

            _animator.SetFloat(AnimVelocityX, Mathf.Abs(_rb2d.velocity.x));
            _animator.SetFloat(AnimVelocityY, _rb2d.velocity.y);
        }

        private void UpdatePerfectReception()
        {
            if (_perfectReceptionCooldownRemaining >= 0)
            {
                _perfectReceptionCooldownRemaining -= Time.deltaTime;
            }

            if (!_isPerfectReception) return;

            _perfectReceptionTimeRemaining -= Time.deltaTime;
            if (_perfectReceptionTimeRemaining <= 0 || !_isGrounded)
            {
                _isPerfectReception = false;
            }
        }

        private void UpdateDash()
        {
            float direction = _move.x > 0f ? 1f : -1f;
            _rb2d.velocity = new Vector2(direction * _blop.DashSpeed * Time.fixedDeltaTime, _rb2d.velocity.y);
            _dashTimeRemaining -= Time.fixedDeltaTime;

            if (_dashTimeRemaining <= 0)
            {
                _isDashing = false;
                _canMove = true;
            }
        }

        #endregion

        #region ===== BALL =====

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (!other.gameObject.CompareTag("Ball") || !other.gameObject.TryGetComponent(out IBall ball)) return;

            _ball = ball;

            if (_isPerfectReception)
            {
                _ball.PerfectReception();
                _perfectReceptionCount = Mathf.Min(_perfectReceptionCount + 1, PerfectReceptionsForSpecialSpike);

                EventBus.OnPlayerPerfectReception?.Invoke(PlayerId, _perfectReceptionCount);
                PlayerEvents.PerfectReception(_blop);
            }
            else if (_isDashing)
            {
                _ball.DashReception();
            }
            else if (_isAbsorbing)
            {
                AbsorbBall();
                PlayerEvents.Absorb(_blop, _ball.transform);
            }
            else if (_isSpecialSpike)
            {
                _rb2d.constraints = RigidbodyConstraints2D.FreezeAll;
                AbsorbBall();

                PlayerEvents.AbsorbSpecialSpike(_blop, _ball.transform);
                EventBus.OnAbsorbedSpecialSpike?.Invoke(PlayerId, _blop.BlopType);
            }
            else
            {
                _isPunchingBall = true;
                CancelInvoke(nameof(EndPunchingBall));
                Invoke(nameof(EndPunchingBall), PunchAnimationDuration);
            }

            EventBus.OnPlayerTouchedBall?.Invoke(PlayerId);
            PlayerEvents.Punch(_blop);
        }

        private void AbsorbBall()
        {
            _hasTheBall = true;
            _ball.Absorb(transform);
        }

        private void ThrowBall()
        {
            if (_ball != null)
            {
                _ball.Drawn(_move);
                PlayerEvents.Drawn(_blop, _ball.transform);
            }

            _hasTheBall = false;
            _ball = null;
            _animator.SetTrigger(AnimDrawnBall);
        }

        /// <summary>Lâche la balle sans la lancer (faute, mort) pour qu'elle ne reste pas figée sur le blop.</summary>
        private void ReleaseBall()
        {
            // La balle peut déjà être détruite (point marqué) : test de nullité Unity, pas C#
            if (_hasTheBall && _ball is Object ballObject && ballObject)
            {
                _ball.Release();
            }

            _hasTheBall = false;
            _isAbsorbing = false;
            _ball = null;
        }

        private void EndPunchingBall()
        {
            _isPunchingBall = false;
        }

        #endregion

        #region ===== INPUTS =====

        public void GetJoystickReadValue(Vector2 move)
        {
            if (_canMove)
            {
                _move = move;
            }
        }

        public void GetWestButtonReadValue(float buttonValue)
        {
            if (Mathf.Approximately(buttonValue, 1))
            {
                if (CanDash())
                {
                    StartDash();
                }
                else if (CanPerfectReception())
                {
                    _isPerfectReception = true;
                    _perfectReceptionTimeRemaining = _blop.PerfectReceptionDuration;
                    _perfectReceptionCooldownRemaining = _blop.PerfectReceptionCooldown;
                }

                if (!_isGrounded && !IsWalled && !_hasTheBall)
                {
                    _isAbsorbing = true;
                    PlayerEvents.CanAbsorb(_blop);
                }
            }
            else if (Mathf.Approximately(buttonValue, 0) && !_isSpecialSpike)
            {
                _isAbsorbing = false;

                if (_hasTheBall)
                {
                    ThrowBall();
                }
            }
        }

        public void GetEastButtonReadValue(float buttonValue)
        {
            if (!Mathf.Approximately(buttonValue, 1)) return;

            if (CanSpecialSpike && MatchManager.Instance != null && PlayerId == MatchManager.Instance.BallSide)
            {
                _isSpecialSpike = true;
                _perfectReceptionCount = 0;

                EventBus.OnSpecialSpikeActivated?.Invoke(PlayerId, _blop.BlopType);
                EventBus.OnPlayerPerfectReception?.Invoke(PlayerId, _perfectReceptionCount);

                PlayerEvents.ActiveSpecialSpike(_blop);
            }

            if (_isSpecialSpike && _hasTheBall && _ball != null)
            {
                _blop.SpecialSpike(gameObject, _ball.gameObject, _move);
                _ball = null;

                PlayerEvents.ShootSpecialSpike(_blop);
            }
        }

        public void GetSouthButtonReadValue(float buttonValue)
        {
            if (_hasTheBall) return;

            if (_isGrounded)
            {
                _canDoubleJump = false;
            }

            if (!Mathf.Approximately(buttonValue, 1)) return;

            if (_isGrounded)
            {
                Jump();
                _canDoubleJump = true;
                PlayerEvents.Jump(_blop);
            }
            else if (IsWalled)
            {
                float awayFromWall = _isWalledLeft ? 1f : -1f;
                _rb2d.velocity = new Vector2(awayFromWall, 1f) * _blop.WallJumpForce;
                _canDoubleJump = true;

                PlayerEvents.WallJump(_blop);
                _animator.SetTrigger(AnimJumpingWall);
            }
            else if (_canDoubleJump)
            {
                Jump();
                _canDoubleJump = false;
                PlayerEvents.DoubleJump(_blop);
            }
        }

        public void GetStartButtonReadValue(float buttonValue)
        {
            if (!Mathf.Approximately(buttonValue, 1)) return;

            if (GameManager.Instance != null && !GameManager.IsGamePaused)
            {
                EventBus.OnGamePaused?.Invoke();
            }
            else
            {
                EventBus.OnGameResumed?.Invoke();
            }
        }

        #endregion

        #region ===== ACTIONS =====

        private bool CanDash()
        {
            return !_isDashing && _isGrounded && _dashCooldownRemaining <= 0 && HasHorizontalInput;
        }

        private void StartDash()
        {
            _isDashing = true;
            _dashTimeRemaining = _blop.DashDuration;
            _dashCooldownRemaining = _blop.DashCooldown;
            _canMove = false;

            PlayerEvents.Dash(_blop);
        }

        private bool CanPerfectReception()
        {
            return !_isPerfectReception && _isGrounded && _perfectReceptionCooldownRemaining <= 0
                   && !HasHorizontalInput && _playerCountTouchBall.CurrentTouchCount == 0 && !_isSpecialSpike;
        }

        private void Jump()
        {
            _rb2d.velocity = new Vector2(_rb2d.velocity.x, _blop.JumpForce);
            _animator.SetTrigger(AnimJumping);
        }

        private void Raycasts()
        {
            bool lastIsGrounded = _isGrounded;
            bool lastIsWalledLeft = _isWalledLeft;
            bool lastIsWalledRight = _isWalledRight;

            float groundedLength = _hasTheBall ? _blop.RayGroundedLengthHaveTheBall : _blop.RayGroundedLength;
            _isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundedLength, _blop.GroundLayer);

            _isWalledLeft = _move.x < -StickDeadZone && WallRaycast(Vector2.left);
            _isWalledRight = _move.x > StickDeadZone && WallRaycast(Vector2.right);

            if (_isGrounded && !lastIsGrounded)
            {
                PlayerEvents.Land(_blop);
            }

            if (_isWalledLeft && !lastIsWalledLeft || _isWalledRight && !lastIsWalledRight)
            {
                PlayerEvents.IsWalled(_blop);
            }
        }

        private bool WallRaycast(Vector2 direction)
        {
            Vector2 origin = (Vector2)transform.position + Vector2.up * WallRayHeightOffset;
            return Physics2D.Raycast(origin, direction, _blop.RayWalledLength, _blop.WallLayer);
        }

        #endregion

        #region ===== EVENTS =====

        private void OnEnable()
        {
            ResetSpecialSpikeState();
            _laserTrigger.SetActive(true);

            EventBus.OnPlayerScored += PlayerScored;
            EventBus.OnFoul += Foul;
            EventBus.OnMatchOver += MatchOver;

            PlayerEvents.Appears(_blop);
        }

        private void OnDisable()
        {
            EventBus.OnPlayerScored -= PlayerScored;
            EventBus.OnFoul -= Foul;
            EventBus.OnMatchOver -= MatchOver;
        }

        private void PlayerScored(int playerId) => ResetSpecialSpikeState();

        private void Foul() => ResetSpecialSpikeState();

        private void MatchOver(int playerId)
        {
            _playerInputHandler.InputAreEnable = false;
            _move = Vector2.zero;
            _animator.SetTrigger(PlayerId == playerId ? AnimWinMatch : AnimLoseMatch);
        }

        #endregion

        #region ===== ANIMATOR =====

        private void UpdateAnimatorStates()
        {
            _animator.SetBool(AnimIsGrounded, _isGrounded);
            _animator.SetBool(AnimIsWalled, IsWalled);
            _animator.SetBool(AnimIsDashing, _isDashing);
            _animator.SetBool(AnimIsPerfectReception, _isPerfectReception);
            _animator.SetBool(AnimIsPunchingBall, _isPunchingBall);
            _animator.SetBool(AnimIsAbsorbing, _isAbsorbing);
            _animator.SetBool(AnimHasTheBall, _hasTheBall);
        }

        private void FlipSprite(float movement)
        {
            if (IsWalled) return;

            if (movement > StickDeadZone)
            {
                transform.rotation = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z);
            }
            else if (movement < -StickDeadZone)
            {
                transform.rotation = Quaternion.Euler(0, 180, transform.rotation.eulerAngles.z);
            }
        }

        #endregion
    }
}
