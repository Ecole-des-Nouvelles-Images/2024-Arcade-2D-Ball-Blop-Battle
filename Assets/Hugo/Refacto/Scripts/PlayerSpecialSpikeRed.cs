using Hugo.Prototype.Scripts.Ball;
using Hugo.Prototype.Scripts.Player;
using Int.Scripts.Utils;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Hugo.Refacto.Scripts
{
    public class PlayerSpecialSpikeRed : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameObject _fakeBall;
        
        [Header("Debug")]
        [SerializeField] private BallController _ballController;
        [SerializeField] private Vector2 _direction;
        [SerializeField] private float _speed;
        [SerializeField] private NewBlopController _newBlopController;
        [SerializeField] private PlayerInput _playerInput;
        
        private int _winningIndex;
        private int _drawnBallCount;
        
        public void Setup(NewBlopController newBlopController, BallController ballController, float speed)
        {
            _newBlopController = newBlopController;
            _ballController = ballController;
            _speed = speed;
            _playerInput = GetComponentInParent<PlayerInputHandler>().PlayerInput;
            
            _winningIndex = Random.Range(0, 3);
            _drawnBallCount = 0;
            
            DrawnBalls(new InputAction.CallbackContext());
            
            _playerInput.actions["LeftJoystick"].performed += LeftJoystick;
            _playerInput.actions["EastButton"].performed += DrawnBalls;
            EventBus.OnSpecialSpikeActivated += SpecialSpikeActivated;
        }

        private void OnDisable()
        {
            _playerInput.actions["LeftJoystick"].performed -= LeftJoystick;
            _playerInput.actions["EastButton"].performed -= DrawnBalls;
            EventBus.OnSpecialSpikeActivated -= SpecialSpikeActivated;
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
                go.GetComponent<FakeBallHandler>().Setup(shootDirection, _speed);
                Debug.Log("Faux ballon tiré.");
            }

            _drawnBallCount++;

            if (_drawnBallCount >= 3)
            {
                _newBlopController.ResetSpecialSpikeState();
                Destroy(gameObject);
            }
        }
        
        private void SpecialSpikeActivated()
        {
            _newBlopController.ResetSpecialSpikeState();
            Destroy(gameObject);
        }
    }
}