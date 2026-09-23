using Player;
using Player.ScriptableObjects;
using UnityEngine;
using UnityEngine.InputSystem;
using Utils;

namespace SpecialSpikes
{
    public class PlayerSpecialSpikeGreen : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private float _speed;
        
        [Header("Debug")]
        [SerializeField] private BallSpecialSpikeGreen _ballSpecialSpikeGreen;
        [SerializeField] private PlayerInput _playerInput;
        
        public void Setup(BallSpecialSpikeGreen ballSpecialSpikeGreen)
        {
            _ballSpecialSpikeGreen = ballSpecialSpikeGreen;
            _playerInput = GetComponentInParent<PlayerInputHandler>().PlayerInput;
            
            _playerInput.actions["EastButton"].performed += ActiveSecondHit;
            EventBus.OnSpecialSpikeActivated += SpecialSpikeActivated;
        }
        
        private void OnDisable()
        {
            _playerInput.actions["EastButton"].performed -= ActiveSecondHit;
            EventBus.OnSpecialSpikeActivated -= SpecialSpikeActivated;
        }

        private void ActiveSecondHit(InputAction.CallbackContext context)
        {
            if (_ballSpecialSpikeGreen) _ballSpecialSpikeGreen.SecondHit(_speed);
            Destroy(gameObject);
        }

        private void SpecialSpikeActivated(int playerId, BlopType blopType)
        {
            Destroy(gameObject);
        }
    }
}