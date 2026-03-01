using Hugo.Prototype.Scripts.Player;
using Int.Scripts.Utils;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Hugo.Refacto.Scripts
{
    public class PlayerSpecialSpikeGreen : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private float _speed;
        
        [Header("References")]
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
            _ballSpecialSpikeGreen.SecondHit(_speed);
            Destroy(gameObject);
        }

        private void SpecialSpikeActivated()
        {
            Destroy(gameObject);
        }
    }
}