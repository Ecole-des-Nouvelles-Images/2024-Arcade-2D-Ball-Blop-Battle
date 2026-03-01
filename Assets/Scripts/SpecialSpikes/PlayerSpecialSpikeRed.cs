using Balls;
using Player;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using Utils;

namespace SpecialSpikes
{
    public class PlayerSpecialSpikeRed : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameObject _fakeBall;
        
        [Header("Debug")]
        [SerializeField] private BallController _ballController;
        [SerializeField] private Vector2 _direction;
        [SerializeField] private float _speed;
        [FormerlySerializedAs("blopController")] [FormerlySerializedAs("_newBlopController")] [SerializeField] private PlayerController playerController;
        [SerializeField] private PlayerInput _playerInput;
        
        private int _winningIndex;
        private int _drawnBallCount;
        
        public void Setup(PlayerController playerController, BallController ballController, Vector2 direction, float speed)
        {
            this.playerController = playerController;
            _ballController = ballController;
            _direction = direction;
            _speed = speed;
            _playerInput = GetComponentInParent<PlayerInputHandler>().PlayerInput;
            
            _winningIndex = Random.Range(0, 3);
            _drawnBallCount = 0;
            
            _playerInput.actions["LeftJoystick"].performed += LeftJoystick;
            _playerInput.actions["EastButton"].performed += DrawnBalls;
            EventBus.OnSpecialSpikeActivated += SpecialSpikeActivated;
            EventBus.OnPlayerScored += PlayerScored;
            
            DrawnBalls(new InputAction.CallbackContext());
        }

        private void OnDisable()
        {
            _playerInput.actions["LeftJoystick"].performed -= LeftJoystick;
            _playerInput.actions["EastButton"].performed -= DrawnBalls;
            EventBus.OnSpecialSpikeActivated -= SpecialSpikeActivated;
            EventBus.OnPlayerScored -= PlayerScored;
        }

        private void LeftJoystick(InputAction.CallbackContext context)
        {
            _direction = context.ReadValue<Vector2>();
        }
        
        private void DrawnBalls(InputAction.CallbackContext context)
        {
            Vector2 shootDirection = (_direction == Vector2.zero) ? Vector2.up : _direction;

            if (_drawnBallCount == _winningIndex)
            {
                _ballController.DrawnSpecialSpike(shootDirection, _speed);
                Debug.Log("Vrai ballon tiré !");
            }
            else
            {
                GameObject go = Instantiate(_fakeBall, transform.position, transform.rotation);
                go.GetComponent<FakeBallController>().Setup(shootDirection, _speed);
                Debug.Log("Faux ballon tiré.");
            }

            _drawnBallCount++;

            if (_drawnBallCount >= 3)
            {
                playerController.ResetSpecialSpikeState();
                Destroy(gameObject);
            }
        }
        
        private void SpecialSpikeActivated()
        {
            playerController.ResetSpecialSpikeState();
            Destroy(gameObject);
        }
        
        private void PlayerScored(int obj)
        {
            playerController.ResetSpecialSpikeState();
            Destroy(gameObject);
        }
    }
}