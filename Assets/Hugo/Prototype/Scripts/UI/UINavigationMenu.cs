using UnityEngine;
using UnityEngine.InputSystem;

namespace Hugo.Prototype.Scripts.UI
{
    public class UINavigationMenu : MonoBehaviour
    {
        private PlayerInput _playerInput;
        
        [Header("Links")]
        [SerializeField] private GameObject _panelMainMenu;
        [SerializeField] private GameObject _panelCurrent;

        private void Awake()
        {
            _playerInput = GetComponent<PlayerInput>();
        }

        private void Update()
        {
            if (_playerInput.actions["UI/Cancel"].triggered)
            {
                _panelCurrent.SetActive(false);
                _panelMainMenu.SetActive(true);
            }
        }
    }
}
