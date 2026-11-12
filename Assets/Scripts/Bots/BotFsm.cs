using UnityEngine;
using Player;

namespace Bots
{
    public enum BotState
    {
        Idle,
        MoveToBall,
        WaitingForPerfectReception,
        AbsorbAndRelease,
        ReturnToCenter
    }

    public class BotFsm : MonoBehaviour
    {
        [Header("===== SETTINGS =====")]
        [SerializeField] private float _tickRate = 0.03f; 
        [SerializeField] private float _reachDistance = 2.2f; 
        [SerializeField] private float _predictionTimeMax = 3.0f; 
        [SerializeField] private int _simulationSteps = 60; 
        [SerializeField] private float _centerPositionX = 4f; 
        [SerializeField] private float _maxHorizontalSpeedForReception = 6.5f; 
        
        [Header("===== RECEPTION TIMINGS =====")]
        private const float ReceptionDuration = 0.25f;
        private const float ReceptionCooldown = 0.75f;
        private float _receptionCooldownTimer = 0f;

        [Header("===== AIM RANDOMNESS =====")]
        [SerializeField, Range(0f, 0.5f)] private float _aimRandomnessX = 0.15f; 
        [SerializeField, Range(0f, 0.4f)] private float _aimRandomnessY = 0.2f;  

        [Header("===== DEBUGS =====")]
        [SerializeField] private PlayerController _playerController;
        [SerializeField] private BotState _currentState;
        [SerializeField] private Transform _targetBall;
        [SerializeField] private Transform _targetEnemy; 
        
        private PlayerCountTouchBall _touchCountComponent;
        private float _timer;
        private Rigidbody2D _ballRb;
        private Vector2 _targetPosition;

        public void Setup(PlayerController playerController)
        {
            _playerController = playerController;
            _currentState = BotState.Idle;
            
            if (_playerController != null)
            {
                _touchCountComponent = _playerController.GetComponent<PlayerCountTouchBall>();
            }

            FindEnemyPlayer();
        }

        private void Update()
        {
            if (!_playerController || !_playerController.gameObject.activeInHierarchy) return;

            if (_receptionCooldownTimer > 0f)
            {
                _receptionCooldownTimer -= Time.deltaTime;
            }
            
            FindClosestBall();
            
            if (!_targetEnemy)
            {
                FindEnemyPlayer();
            }

            _timer += Time.deltaTime;
            if (_timer >= _tickRate)
            {
                _timer = 0;
                ExecuteFsmLogic();
            }
        }

        private void FindClosestBall()
        {
            GameObject ballObject = GameObject.FindWithTag("Ball");
    
            if (ballObject)
            {
                _targetBall = ballObject.transform;
                _ballRb = ballObject.GetComponent<Rigidbody2D>();
            }
            else
            {
                _targetBall = null;
                _ballRb = null;
            }
        }

        private void FindEnemyPlayer()
        {
            PlayerController[] allPlayers = FindObjectsByType<PlayerController>(FindObjectsSortMode.None);
            foreach (var p in allPlayers)
            {
                if (p.PlayerId != _playerController.PlayerId)
                {
                    _targetEnemy = p.transform;
                    break;
                }
            }
        }

        private bool TryPredictGroundLanding(out Vector2 landingPos, out float timeToLand)
        {
            landingPos = _targetBall.position;
            timeToLand = 0f;

            if (!_targetBall || !_ballRb) return false;

            Vector2 currentPos = _targetBall.position;
            Vector2 currentVelocity = _ballRb.velocity;
            float gravity = Physics2D.gravity.y * _ballRb.gravityScale;
            float dt = _predictionTimeMax / _simulationSteps;
            float groundY = _playerController.transform.position.y + 0.3f;

            for (int i = 1; i <= _simulationSteps; i++)
            {
                currentPos += currentVelocity * dt;
                currentVelocity.y += gravity * dt;

                if (currentPos.x < -8.5f || currentPos.x > 8.5f)
                {
                    currentVelocity.x = -currentVelocity.x;
                    currentPos.x = Mathf.Clamp(currentPos.x, -8.5f, 8.5f);
                }

                if (currentPos.y > 4.8f)
                {
                    currentVelocity.y = -Mathf.Abs(currentVelocity.y);
                }

                if (currentPos.y <= groundY && currentVelocity.y <= 0f)
                {
                    landingPos = currentPos;
                    timeToLand = i * dt;
                    return true;
                }
            }

            return false;
        }

        private Vector2 CalculateDynamicIntercept()
        {
            if (!_targetBall || !_ballRb) return transform.position;

            Vector2 currentPos = _targetBall.position;
            Vector2 currentVelocity = _ballRb.velocity;
            float gravity = Physics2D.gravity.y * _ballRb.gravityScale;
            float dt = _predictionTimeMax / _simulationSteps;

            Vector2 bestIntercept = currentPos;
            float minScore = float.MaxValue;

            for (int i = 0; i < _simulationSteps; i++)
            {
                currentPos += currentVelocity * dt;
                currentVelocity.y += gravity * dt;

                if (currentPos.x < -8.5f || currentPos.x > 8.5f)
                {
                    currentVelocity.x = -currentVelocity.x;
                }

                float distToBot = Vector2.Distance(currentPos, _playerController.transform.position);
                float sidePenalty = (currentPos.x < 0f) ? 15f : 0f;
                float heightFactor = Mathf.Abs(currentPos.y - _playerController.transform.position.y);
                float totalScore = distToBot + sidePenalty + (heightFactor * 0.2f);

                if (totalScore < minScore)
                {
                    minScore = totalScore;
                    bestIntercept = currentPos;
                }
            }

            return bestIntercept;
        }

        private Vector2 CalculateRandomizedLaunchDirection()
        {
            if (!_targetEnemy)
            {
                float fallbackX = (_playerController.PlayerId == 1) ? 1f : -1f;
                return new Vector2(fallbackX, 0.6f).normalized;
            }

            Vector2 botPos = _playerController.transform.position;
            Vector2 enemyPos = _targetEnemy.position;
            
            float distanceX = enemyPos.x - botPos.x;
            float baseDirX = Mathf.Sign(distanceX);
            float baseDirY = Mathf.Clamp(0.6f + (Mathf.Abs(distanceX) * 0.04f), 0.5f, 0.85f);

            float randomizedX = baseDirX + Random.Range(-_aimRandomnessX, _aimRandomnessX);
            float randomizedY = baseDirY + Random.Range(-_aimRandomnessY, _aimRandomnessY);

            randomizedY = Mathf.Clamp(randomizedY, 0.3f, 0.95f);

            return new Vector2(randomizedX, randomizedY).normalized;
        }

        private void ExecuteFsmLogic()
        {
            if (_targetBall && _targetBall.position.x < 0f && _currentState != BotState.AbsorbAndRelease && _currentState != BotState.WaitingForPerfectReception)
            {
                _currentState = BotState.ReturnToCenter;
            }

            switch (_currentState)
            {
                case BotState.Idle:
                    HandleIdleState();
                    break;
                case BotState.MoveToBall:
                    HandleMoveToBallState();
                    break;
                case BotState.WaitingForPerfectReception:
                    HandleWaitingForPerfectReceptionState();
                    break;
                case BotState.AbsorbAndRelease:
                    HandleAbsorbAndReleaseState();
                    break;
                case BotState.ReturnToCenter:
                    HandleReturnToCenterState();
                    break;
            }
        }

        private void HandleIdleState()
        {
            _playerController.GetJoystickReadValue(Vector2.zero);
            _playerController.GetWestButtonReadValue(0);
            _playerController.GetSouthButtonReadValue(0);

            if (_targetBall)
            {
                if (_targetBall.position.x >= 0f)
                {
                    _currentState = BotState.MoveToBall;
                }
                else
                {
                    _currentState = BotState.ReturnToCenter;
                }
            }
        }

        private void HandleMoveToBallState()
        {
            if (!_targetBall)
            {
                _currentState = BotState.Idle;
                return;
            }

            bool isEngagementOrNoGravity = (_ballRb.gravityScale == 0f || _ballRb.velocity.magnitude < 0.1f);

            if (isEngagementOrNoGravity)
            {
                _targetPosition = _targetBall.position;

                float directionX = _targetPosition.x - _playerController.transform.position.x;
                float heightDifference = _targetBall.position.y - _playerController.transform.position.y;

                float moveInput = Mathf.Abs(directionX) > 0.05f ? Mathf.Sign(directionX) : 0f;
                _playerController.GetJoystickReadValue(new Vector2(moveInput, 0f));

                if (heightDifference > 0.4f && Mathf.Abs(directionX) < 1.5f)
                {
                    _playerController.GetSouthButtonReadValue(1);
                }
                else
                {
                    _playerController.GetSouthButtonReadValue(0);
                }

                float distance = Vector2.Distance(_playerController.transform.position, _targetBall.position);
                if (distance <= _reachDistance || Mathf.Abs(directionX) < 0.6f)
                {
                    _playerController.GetWestButtonReadValue(1); 
                    if (distance <= 1.5f)
                    {
                        _currentState = BotState.AbsorbAndRelease;
                    }
                }
                return;
            }

            bool hasGroundLanding = TryPredictGroundLanding(out Vector2 landingPos, out float timeToLand);
            bool isHorizontalSpeedSlow = Mathf.Abs(_ballRb.velocity.x) <= _maxHorizontalSpeedForReception;
            float distToLandingX = Mathf.Abs(landingPos.x - _playerController.transform.position.x);
            bool hasEnoughTimeToReach = (distToLandingX / 8.0f) <= (timeToLand + 0.15f); // Ajusté à 8.0f vu la vitesse sans inertie

            bool canAttemptReception = (_receptionCooldownTimer <= 0f);
            bool isFirstTouch = (_touchCountComponent == null || _touchCountComponent.CurrentTouchCount == 0);

            if (isFirstTouch && hasGroundLanding && landingPos.x >= 0f && isHorizontalSpeedSlow && hasEnoughTimeToReach && canAttemptReception)
            {
                // ==========================================
                // CAS 2 : RÉCEPTION PARFAITE AU SOL (0 touch)
                // ==========================================
                _targetPosition = landingPos;

                float directionX = _targetPosition.x - _playerController.transform.position.x;
                float moveInput = Mathf.Abs(directionX) > 0.05f ? Mathf.Sign(directionX) : 0f;
                
                _playerController.GetJoystickReadValue(new Vector2(moveInput, 0f));
                _playerController.GetSouthButtonReadValue(0); 

                if (Mathf.Abs(directionX) <= 0.2f)
                {
                    _playerController.GetJoystickReadValue(Vector2.zero);

                    if (timeToLand <= ReceptionDuration)
                    {
                        _playerController.GetWestButtonReadValue(1);
                        _currentState = BotState.WaitingForPerfectReception;
                    }
                    else
                    {
                        _playerController.GetWestButtonReadValue(0);
                    }
                }
            }
            else
            {
                // ==========================================
                // CAS 3 : INTERCEPTION DYNAMIQUE
                // ==========================================
                _targetPosition = CalculateDynamicIntercept();

                float directionX = _targetPosition.x - _playerController.transform.position.x;
                float heightDifference = _targetBall.position.y - _playerController.transform.position.y;

                float moveInput = Mathf.Abs(directionX) > 0.05f ? Mathf.Sign(directionX) : 0f;
                _playerController.GetJoystickReadValue(new Vector2(moveInput, 0f));

                bool isAlignedHorizontally = Mathf.Abs(directionX) < 0.4f;
                bool isBallInAir = heightDifference > 0.6f && heightDifference < 3.0f;

                if (isAlignedHorizontally && isBallInAir)
                {
                    _playerController.GetSouthButtonReadValue(1); 
                }
                else
                {
                    _playerController.GetSouthButtonReadValue(0);
                }

                float distance = Vector2.Distance(_playerController.transform.position, _targetBall.position);
                if (distance <= _reachDistance)
                {
                    _playerController.GetWestButtonReadValue(1); 
                    
                    if (distance <= 1.5f)
                    {
                        _currentState = BotState.AbsorbAndRelease;
                    }
                }
            }
        }

        private void HandleWaitingForPerfectReceptionState()
        {
            _playerController.GetJoystickReadValue(Vector2.zero);
            _playerController.GetSouthButtonReadValue(0);
            _playerController.GetWestButtonReadValue(1);

            if (Mathf.Abs(_targetBall.position.x - _playerController.transform.position.x) > 1.2f)
            {
                _receptionCooldownTimer = ReceptionCooldown;
                _currentState = BotState.MoveToBall;
                return;
            }

            if (_playerController._hasTheBall || (_targetBall.position.y > _playerController.transform.position.y + 0.6f && _ballRb.velocity.y > 0.5f))
            {
                _playerController.GetWestButtonReadValue(0);
                _receptionCooldownTimer = ReceptionCooldown;
                _currentState = BotState.AbsorbAndRelease;
            }
        }

        private void HandleAbsorbAndReleaseState()
        {
            Vector2 launchDirection = CalculateRandomizedLaunchDirection();
            
            _playerController.GetJoystickReadValue(launchDirection);
            _playerController.GetWestButtonReadValue(0); 

            _currentState = BotState.Idle;
        }

        private void HandleReturnToCenterState()
        {
            _playerController.GetWestButtonReadValue(0);
            _playerController.GetSouthButtonReadValue(0);

            float directionX = _centerPositionX - _playerController.transform.position.x;
            
            if (Mathf.Abs(directionX) <= 0.2f)
            {
                _playerController.GetJoystickReadValue(Vector2.zero);
                _currentState = BotState.Idle;
            }
            else
            {
                float moveInput = Mathf.Sign(directionX);
                _playerController.GetJoystickReadValue(new Vector2(moveInput, 0f));
            }
        }
    }
}