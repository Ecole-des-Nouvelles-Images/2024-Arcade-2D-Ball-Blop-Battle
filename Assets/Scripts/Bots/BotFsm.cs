using Balls;
using Managers;
using Player;
using UnityEngine;
using Utils;

namespace Bots
{
    public enum BotState
    {
        Idle,               // Pas de balle : retour au centre de son camp
        Reposition,         // Balle chez l'adversaire : se replacer
        Intercept,          // Balle qui arrive : se placer, sauter, absorber
        PerfectReception,   // Balle qui tombe lentement : réception parfaite au sol
        Absorb,             // En l'air, bouton d'absorption maintenu
        Throw               // Balle absorbée : viser et relâcher avant de toucher le sol
    }

    /// <summary>
    /// IA d'un blop, organisée en trois couches :
    /// Perception (état de la balle perçu avec un temps de réaction + prédiction de trajectoire),
    /// Décision (FSM),
    /// Action (BotActuator, qui envoie les inputs comme une manette).
    /// Fonctionne des deux côtés du filet.
    /// </summary>
    public class BotFsm : MonoBehaviour
    {
        [Header("===== DIFFICULTY =====")]
        [SerializeField] private BotDifficulty _difficulty;

        [Header("===== ARENA =====")]
        [SerializeField] private ArenaBounds _arena = ArenaBounds.Default;
        [Tooltip("Distance au filet de la position de repos.")]
        [SerializeField] private float _homeDistanceFromNet = 4.5f;

        [Header("===== BODY =====")]
        [Tooltip("Hauteur (depuis les pieds) à laquelle le blop touche la balle.")]
        [SerializeField] private float _contactHeight = 0.9f;
        [Tooltip("Distance balle / blop sous laquelle le bot déclenche l'absorption en l'air.")]
        [SerializeField] private float _absorbRange = 1.4f;
        [Tooltip("Hauteur minimale (au-dessus du point de contact) pour sauter vers la balle.")]
        [SerializeField] private float _minJumpHeight = 0.25f;

        [Header("===== PREDICTION =====")]
        [SerializeField] private float _predictionHorizon = 3f;

        [Header("===== PERFECT RECEPTION =====")]
        [SerializeField] private float _maxHorizontalSpeedForReception = 6.5f;
        [SerializeField] private float _receptionAlignTolerance = 0.15f;

        [Header("===== DASH =====")]
        [SerializeField] private float _dashMinDistance = 2.5f;

        [Header("===== AIM =====")]
        [SerializeField] private float _minAimAngle = -40f;
        [SerializeField] private float _maxAimAngle = 75f;
        [SerializeField, Min(2)] private int _aimCandidates = 16;

        [Header("===== DEBUG =====")]
        [SerializeField] private BotState _currentState;
        [SerializeField] private PlayerController _playerController;
        [SerializeField] private PlayerController _enemy;
        [SerializeField] private BaseBallController _ball;
        [SerializeField] private Vector2 _targetPosition;

        // ===== CONSTANTES =====
        private const int HistorySize = 128;
        private const float BallSearchInterval = 0.25f;
        private const float AbsorbTimeout = 0.6f;

        // ===== RÉFÉRENCES =====
        private BotActuator _actuator;
        private Rigidbody2D _playerRb;
        private Rigidbody2D _ballRb;
        private float _ballRadius = 0.45f;
        private float _ballGravityScale = 1f;

        // ===== CÔTÉ =====
        private float _side; // -1 = camp gauche (joueur 1), +1 = camp droit (joueur 2)

        // ===== PERCEPTION =====
        private struct BallSnapshot
        {
            public float Time;
            public Vector2 Position;
            public Vector2 Velocity;
            public float GravityScale;
            public bool IsAbsorbed;
        }

        private readonly BallSnapshot[] _history = new BallSnapshot[HistorySize];
        private int _historyHead;
        private int _historyCount;
        private BallSnapshot _perceived;
        private bool _hasPerception;

        private readonly BallPredictor _predictor = new();
        private readonly BallPredictor _aimPredictor = new();

        // ===== ÉTAT =====
        private float _decisionTimer;
        private float _ballSearchTimer;
        private float _stateEnterTime;
        private float _positionErrorX;
        private bool _perfectReceptionRoll;
        private float _perfectReceptionCooldown;
        private float _perfectReceptionPressTime = -1f;
        private bool _usedDoubleJump;
        private bool _wasInactive;
        private bool _isMatchOver;

        private BotDifficulty Difficulty => _difficulty;
        private Vector2 PlayerPos => _playerController.transform.position;
        private float RunSpeed => _playerController.Blop.Speed * Time.fixedDeltaTime;
        private float ContactY => _arena.GroundY + _contactHeight;
        private int TouchCount => _playerController.CountTouchBall ? _playerController.CountTouchBall.CurrentTouchCount : 0;
        private int MaxTouchCount => _playerController.CountTouchBall ? _playerController.CountTouchBall.MaxTouchCount : 3;

        #region ===== SETUP =====

        /// <param name="difficulty">Difficulté choisie dans la sélection ; null garde celle du prefab.</param>
        public void Setup(PlayerController playerController, BotDifficulty difficulty = null)
        {
            if (difficulty) _difficulty = difficulty;

            if (!_difficulty)
            {
                Debug.LogError($"[{nameof(BotFsm)}] Aucune BotDifficulty assignée (prefab ou sélection) : bot désactivé.", this);
                enabled = false;
                return;
            }

            _playerController = playerController;
            _playerRb = playerController.Rigidbody;
            _actuator = new BotActuator(playerController);

            // Le joueur 1 joue à gauche du filet, le joueur 2 à droite (cf. PlayerOneSide / PlayerTwoSide)
            _side = playerController.PlayerId == 1 ? -1f : 1f;

            ChangeState(BotState.Idle);
            RerollErrors();
        }

        private void OnEnable()
        {
            EventBus.OnPlayerTouchedBall += OnPlayerTouchedBall;
            EventBus.OnMatchOver += OnMatchOver;
        }

        private void OnDisable()
        {
            EventBus.OnPlayerTouchedBall -= OnPlayerTouchedBall;
            EventBus.OnMatchOver -= OnMatchOver;
        }

        private void OnPlayerTouchedBall(int playerId) => RerollErrors();

        private void OnMatchOver(int playerId)
        {
            _isMatchOver = true;
            _actuator?.ReleaseAll();
        }

        private void RerollErrors()
        {
            _positionErrorX = Random.Range(-1f, 1f) * Difficulty.PositioningError;
            _perfectReceptionRoll = Random.value < Difficulty.PerfectReceptionChance;
        }

        #endregion

        #region ===== LOOP =====

        private void Update()
        {
            if (!_playerController) return;

            if (!_playerController.gameObject.activeInHierarchy)
            {
                _wasInactive = true;
                return;
            }

            if (_wasInactive)
            {
                _wasInactive = false;
                _actuator.Invalidate();
                ChangeState(BotState.Idle);
            }

            if (_isMatchOver || GameManager.IsGamePaused) return;

            if (_perfectReceptionCooldown > 0f) _perfectReceptionCooldown -= Time.deltaTime;
            if (_playerController.IsGrounded) _usedDoubleJump = false;

            TrackBall();

            _decisionTimer -= Time.deltaTime;
            if (_decisionTimer > 0f) return;
            _decisionTimer = Difficulty.DecisionInterval;

            Perceive();
            Decide();
            Act();
        }

        #endregion

        #region ===== PERCEPTION =====

        private void TrackBall()
        {
            if (!_ball)
            {
                _ballSearchTimer -= Time.deltaTime;
                if (_ballSearchTimer > 0f) return;
                _ballSearchTimer = BallSearchInterval;

                GameObject ballObject = GameObject.FindWithTag("Ball");
                if (!ballObject || !ballObject.TryGetComponent(out _ball)) return;

                _ballRb = _ball.Rigidbody;
                _ballRadius = _ball.Collider is CircleCollider2D circle
                    ? circle.radius * Mathf.Max(_ball.transform.lossyScale.x, _ball.transform.lossyScale.y)
                    : _ball.Collider.bounds.extents.x;

                _historyCount = 0;
                RerollErrors();
            }

            if (!_enemy || _enemy == _playerController)
            {
                FindEnemy();
            }
        }

        /// <summary>
        /// L'historique est enregistré au pas physique (et non à chaque frame) :
        /// sa durée couverte (HistorySize × fixedDeltaTime) ne dépend donc pas du FPS.
        /// </summary>
        private void FixedUpdate()
        {
            if (!_playerController || !_ball || !_ballRb) return;

            if (_ballRb.gravityScale > 0f) _ballGravityScale = _ballRb.gravityScale;

            _history[_historyHead] = new BallSnapshot
            {
                Time = Time.time,
                Position = _ball.transform.position,
                Velocity = _ballRb.velocity,
                GravityScale = _ballRb.gravityScale,
                IsAbsorbed = _ball.IsAbsorbed
            };
            _historyHead = (_historyHead + 1) % HistorySize;
            _historyCount = Mathf.Min(_historyCount + 1, HistorySize);
        }

        private void FindEnemy()
        {
            foreach (PlayerController player in FindObjectsByType<PlayerController>(FindObjectsInactive.Include, FindObjectsSortMode.None))
            {
                if (player != _playerController)
                {
                    _enemy = player;
                    return;
                }
            }
        }

        /// <summary>Le bot ne voit la balle qu'avec un retard égal à son temps de réaction.</summary>
        private bool TryGetDelayedSnapshot(out BallSnapshot snapshot)
        {
            float targetTime = Time.time - Difficulty.ReactionTime;

            for (int i = 1; i <= _historyCount; i++)
            {
                int index = (_historyHead - i + HistorySize) % HistorySize;
                if (_history[index].Time <= targetTime)
                {
                    snapshot = _history[index];
                    return true;
                }
            }

            snapshot = default;
            return false;
        }

        private void Perceive()
        {
            _hasPerception = _ball && TryGetDelayedSnapshot(out _perceived);
            if (!_hasPerception) return;

            float delay = Time.time - _perceived.Time;
            float gravity = Physics2D.gravity.y * _perceived.GravityScale;

            _predictor.Simulate(_perceived.Position, _perceived.Velocity, gravity, _ballRadius,
                _arena, _predictionHorizon, Time.fixedDeltaTime, -delay);
        }

        private bool IsOnMySide(float x) => (x - _arena.NetX) * _side > 0f;

        private float HomeX => _arena.NetX + _side * _homeDistanceFromNet;

        /// <summary>Empêche le bot de viser une position de l'autre côté du filet.</summary>
        private float ClampToMySide(float x)
        {
            float minDistanceFromNet = _arena.NetHalfWidth + 0.5f;
            float distance = Mathf.Max((x - _arena.NetX) * _side, minDistanceFromNet);
            return _arena.NetX + distance * _side;
        }

        /// <summary>La balle est (ou sera) dans notre camp avant de toucher le sol.</summary>
        private bool BallThreatensMySide()
        {
            if (IsOnMySide(_perceived.Position.x)) return true;

            for (int i = 0; i < _predictor.Count; i++)
            {
                if (IsOnMySide(_predictor[i].Position.x)) return true;
            }
            return false;
        }

        /// <summary>Position de la balle "maintenant" selon la prédiction.</summary>
        private Vector2 PredictedBallNow()
        {
            int index = _predictor.FirstFutureIndex();
            return index >= 0 ? _predictor[index].Position : _perceived.Position;
        }

        /// <summary>Premier moment où la balle descend à hauteur de contact du blop, dans notre camp.</summary>
        private bool TryGetContactSample(out BallSample contact)
        {
            int start = _predictor.FirstFutureIndex();
            if (start >= 0)
            {
                for (int i = start; i < _predictor.Count; i++)
                {
                    BallSample sample = _predictor[i];
                    if (sample.Velocity.y < 0f && sample.Position.y - _ballRadius <= ContactY)
                    {
                        contact = sample;
                        return IsOnMySide(sample.Position.x);
                    }
                }
            }

            contact = default;
            return false;
        }

        /// <summary>
        /// Premier point de la trajectoire, dans notre camp, que le blop peut atteindre à temps
        /// (déplacement horizontal et montée du saut se font en parallèle).
        /// </summary>
        private bool TryFindIntercept(out BallSample intercept)
        {
            float maxHeight = JumpApexHeight();
            int start = _predictor.FirstFutureIndex();

            if (start >= 0)
            {
                for (int i = start; i < _predictor.Count; i++)
                {
                    BallSample sample = _predictor[i];
                    if (!IsOnMySide(sample.Position.x)) continue;

                    float height = sample.Position.y - ContactY;
                    if (height < _minJumpHeight || height > maxHeight) continue;

                    float runTime = Mathf.Abs(sample.Position.x - PlayerPos.x) / RunSpeed;
                    float riseTime = TimeToRise(height);

                    if (Mathf.Max(runTime, riseTime) <= sample.Time)
                    {
                        intercept = sample;
                        return true;
                    }
                }
            }

            intercept = default;
            return false;
        }

        private float PlayerGravity => Mathf.Abs(Physics2D.gravity.y * _playerRb.gravityScale);

        private float JumpApexHeight()
        {
            float v = _playerController.Blop.JumpForce;
            return v * v / (2f * PlayerGravity);
        }

        /// <summary>Temps pour qu'un saut atteigne la hauteur donnée (temps d'apogée si inatteignable).</summary>
        private float TimeToRise(float height)
        {
            float v = _playerController.Blop.JumpForce;
            float g = PlayerGravity;
            float discriminant = v * v - 2f * g * height;
            return discriminant <= 0f ? v / g : (v - Mathf.Sqrt(discriminant)) / g;
        }

        #endregion

        #region ===== DECISION =====

        private void Decide()
        {
            BotState next = _currentState;

            if (_playerController.HasTheBall)
            {
                next = BotState.Throw;
            }
            else if (!_hasPerception)
            {
                next = BotState.Idle;
            }
            else if (_perceived.IsAbsorbed)
            {
                next = BotState.Reposition;
            }
            else if (_currentState == BotState.Absorb && IsAbsorbStillValid())
            {
                next = BotState.Absorb;
            }
            else if (_currentState == BotState.PerfectReception && IsPerfectReceptionStillValid())
            {
                next = BotState.PerfectReception;
            }
            else if (BallThreatensMySide())
            {
                next = CanTryPerfectReception() ? BotState.PerfectReception : BotState.Intercept;
            }
            else
            {
                next = BotState.Reposition;
            }

            if (next != _currentState) ChangeState(next);
        }

        private void ChangeState(BotState next)
        {
            // Sortie : ne jamais garder un bouton appuyé d'un état à l'autre
            if (_actuator != null && _currentState == BotState.Absorb && next != BotState.Throw)
            {
                _actuator.SetWest(false);
            }

            _currentState = next;
            _stateEnterTime = Time.time;
            _perfectReceptionPressTime = -1f;
        }

        private float TimeInState => Time.time - _stateEnterTime;

        private bool IsAbsorbStillValid()
        {
            if (_playerController.IsGrounded || TimeInState > AbsorbTimeout) return false;
            return Vector2.Distance(PredictedBallNow(), PlayerPos + Vector2.up * _contactHeight) < _absorbRange * 2f;
        }

        private bool CanTryPerfectReception()
        {
            if (!_perfectReceptionRoll || _perfectReceptionCooldown > 0f) return false;
            if (TouchCount != 0 || !_playerController.IsGrounded) return false;
            if (_perceived.GravityScale == 0f) return false;
            if (!TryGetContactSample(out BallSample contact)) return false;
            if (Mathf.Abs(contact.Velocity.x) > _maxHorizontalSpeedForReception) return false;

            float runTime = Mathf.Abs(contact.Position.x - PlayerPos.x) / RunSpeed;
            return runTime + Difficulty.DecisionInterval <= contact.Time;
        }

        private bool IsPerfectReceptionStillValid()
        {
            if (_perfectReceptionPressTime >= 0f)
            {
                // Bouton pressé : on attend la fin de la fenêtre de réception
                return Time.time - _perfectReceptionPressTime < _playerController.Blop.PerfectReceptionDuration + 0.1f;
            }

            return TouchCount == 0 && _playerController.IsGrounded && TryGetContactSample(out _);
        }

        #endregion

        #region ===== ACTION =====

        private void Act()
        {
            switch (_currentState)
            {
                case BotState.Idle:
                case BotState.Reposition:
                    ActReposition();
                    break;
                case BotState.Intercept:
                    ActIntercept();
                    break;
                case BotState.PerfectReception:
                    ActPerfectReception();
                    break;
                case BotState.Absorb:
                    ActAbsorb();
                    break;
                case BotState.Throw:
                    ActThrow();
                    break;
            }
        }

        private void MoveTowardX(float targetX, float deadZone = 0.1f)
        {
            _targetPosition = new Vector2(targetX, _targetPosition.y);
            float dx = targetX - PlayerPos.x;

            // Ralentit à l'approche pour éviter d'osciller autour de la cible
            float input = Mathf.Abs(dx) <= deadZone ? 0f : Mathf.Clamp(dx / 0.5f, -1f, 1f);
            if (Mathf.Abs(input) > 0f && Mathf.Abs(input) < 0.2f) input = 0.2f * Mathf.Sign(input);

            _actuator.SetMoveX(input);
        }

        private void ActReposition()
        {
            _actuator.SetSouth(false);
            _actuator.SetWest(false);
            _targetPosition = new Vector2(HomeX, _arena.GroundY);
            MoveTowardX(HomeX, 0.2f);
        }

        private void ActIntercept()
        {
            bool hasIntercept = TryFindIntercept(out BallSample target);

            if (!hasIntercept)
            {
                // Inatteignable en l'air : se placer sous le point de contact, sinon sous la balle
                if (TryGetContactSample(out BallSample contact)) target = contact;
                else target = new BallSample { Position = PredictedBallNow(), Time = 0f };
            }

            float targetX = ClampToMySide(target.Position.x + _positionErrorX);
            _targetPosition = new Vector2(targetX, target.Position.y);
            MoveTowardX(targetX);

            float dx = targetX - PlayerPos.x;
            float height = target.Position.y - ContactY;

            if (_playerController.IsGrounded)
            {
                // Dash si la course à pied ne suffit pas
                bool needsDash = Difficulty.UseDash && Mathf.Abs(dx) > _dashMinDistance
                                 && Mathf.Abs(dx) / RunSpeed > target.Time;
                if (needsDash) _actuator.TapWest();
                else _actuator.SetWest(false);

                // Saut : déclenché pour arriver à hauteur de balle au bon moment
                float riseTime = TimeToRise(height);
                bool canReachHorizontally = Mathf.Abs(dx) <= Mathf.Max(0.35f, RunSpeed * riseTime);
                bool isJumpTime = target.Time <= riseTime + Difficulty.DecisionInterval * 0.5f;

                if (height > _minJumpHeight && canReachHorizontally && isJumpTime && !needsDash)
                {
                    _actuator.TapSouth();
                }
                else
                {
                    _actuator.SetSouth(false);
                }
                return;
            }

            // En l'air
            Vector2 ballNow = PredictedBallNow();
            Vector2 contactPoint = PlayerPos + Vector2.up * _contactHeight;

            if (Vector2.Distance(ballNow, contactPoint) < _absorbRange && TouchCount < MaxTouchCount - 1)
            {
                _actuator.TapWest();
                ChangeState(BotState.Absorb);
                return;
            }

            // Double saut si la balle reste au-dessus et que le premier saut s'essouffle
            bool ballAbove = ballNow.y - contactPoint.y > 0.6f && Mathf.Abs(ballNow.x - PlayerPos.x) < 1f;
            if (!_usedDoubleJump && ballAbove && _playerRb.velocity.y < 1f)
            {
                _usedDoubleJump = true;
                _actuator.TapSouth();
            }
            else
            {
                _actuator.SetSouth(false);
            }
        }

        private void ActPerfectReception()
        {
            _actuator.SetSouth(false);

            if (_perfectReceptionPressTime >= 0f)
            {
                _actuator.SetMove(Vector2.zero);
                return;
            }

            if (!TryGetContactSample(out BallSample contact))
            {
                ChangeState(BotState.Intercept);
                return;
            }

            float targetX = contact.Position.x + _positionErrorX * 0.5f;
            _targetPosition = new Vector2(targetX, contact.Position.y);

            if (Mathf.Abs(targetX - PlayerPos.x) > _receptionAlignTolerance)
            {
                _actuator.SetWest(false);
                MoveTowardX(targetX, _receptionAlignTolerance);
                return;
            }

            // Immobile : le PlayerController n'accepte la réception parfaite que sans mouvement
            _actuator.SetMove(Vector2.zero);

            float window = _playerController.Blop.PerfectReceptionDuration;
            if (contact.Time <= window * 0.6f)
            {
                _actuator.TapWest();
                _actuator.SetWest(false);
                _perfectReceptionPressTime = Time.time;
                _perfectReceptionCooldown = _playerController.Blop.PerfectReceptionCooldown;
            }
        }

        private void ActAbsorb()
        {
            _actuator.SetSouth(false);
            _actuator.SetWest(true);
            MoveTowardX(PredictedBallNow().x + _positionErrorX * 0.3f);
        }

        private void ActThrow()
        {
            // Viser puis relâcher dans le même tick : garder la balle au sol est une faute
            Vector2 direction = ChooseThrowDirection();
            _actuator.SetMove(direction);

            // Le tir part sur le relâchement : s'assurer qu'il y a bien un front descendant
            if (!_actuator.IsWestPressed) _actuator.SetWest(true);
            _actuator.SetWest(false);
        }

        /// <summary>
        /// Simule plusieurs angles de tir et note chacun (utility) :
        /// la balle doit passer le filet et tomber loin de l'adversaire, idéalement vite.
        /// </summary>
        private Vector2 ChooseThrowDirection()
        {
            float towardEnemy = -_side;
            Vector2 fallback = new Vector2(towardEnemy, 1f).normalized;

            if (!_ball || !_ballRb) return fallback;

            Vector2 origin = PlayerPos;
            float speed = _ball.DrawnImpulse / _ballRb.mass;
            float gravity = Physics2D.gravity.y * _ballGravityScale;
            float enemyX = _enemy && _enemy.gameObject.activeInHierarchy
                ? _enemy.transform.position.x
                : _arena.NetX - _side * _homeDistanceFromNet;
            float halfWidth = (_arena.RightWallX - _arena.LeftWallX) * 0.5f;
            float noiseAmplitude = 1f - Difficulty.AimSkill;

            float bestScore = float.MinValue;
            float bestAngle = float.NaN;

            for (int i = 0; i < _aimCandidates; i++)
            {
                float angle = Mathf.Lerp(_minAimAngle, _maxAimAngle, i / (float)(_aimCandidates - 1));
                Vector2 direction = AngleToDirection(angle, towardEnemy);

                _aimPredictor.Simulate(origin, direction * speed, gravity, _ballRadius,
                    _arena, _predictionHorizon, Time.fixedDeltaTime);

                if (!_aimPredictor.HasLanding || _aimPredictor.HitNet) continue;
                if (IsOnMySide(_aimPredictor.Landing.Position.x)) continue;

                float distanceFromEnemy = Mathf.Abs(_aimPredictor.Landing.Position.x - enemyX) / halfWidth;
                float speedBonus = 1f / (1f + _aimPredictor.Landing.Time);
                float score = distanceFromEnemy + 0.3f * speedBonus + Random.value * noiseAmplitude;

                if (score > bestScore)
                {
                    bestScore = score;
                    bestAngle = angle;
                }
            }

            if (float.IsNaN(bestAngle)) return fallback;

            float jitter = Random.Range(-1f, 1f) * Difficulty.AimErrorDegrees;
            return AngleToDirection(bestAngle + jitter, towardEnemy);
        }

        private static Vector2 AngleToDirection(float angleDegrees, float towardX)
        {
            float rad = angleDegrees * Mathf.Deg2Rad;
            return new Vector2(Mathf.Cos(rad) * towardX, Mathf.Sin(rad));
        }

        #endregion

        #region ===== GIZMOS =====

        private void OnDrawGizmos()
        {
            if (!_playerController) return;

            _predictor.DrawGizmos(Color.yellow);

            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(_targetPosition, 0.25f);
            Gizmos.DrawLine(_playerController.transform.position, _targetPosition);
        }

        #endregion
    }
}
