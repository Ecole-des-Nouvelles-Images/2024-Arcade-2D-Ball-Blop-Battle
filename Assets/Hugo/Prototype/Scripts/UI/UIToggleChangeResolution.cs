using UnityEngine;
using UnityEngine.UI;

namespace Hugo.Prototype.Scripts.UI
{
    public class UIToggleChangeResolution : MonoBehaviour
    {
        [Header("Toggles")]
        [SerializeField] private Toggle _toggleOne;
        [SerializeField] private Toggle _toggleTwo;
        [SerializeField] private Toggle _toggleThree;

        private void Start()
        {
            _toggleOne.onValueChanged.AddListener(OnToggleOneValueChanged);
            _toggleTwo.onValueChanged.AddListener(OnToggleTwoValueChanged);
            _toggleThree.onValueChanged.AddListener(OnToggleThreeValueChanged);
        }

        private void OnDestroy()
        {
            _toggleOne.onValueChanged.RemoveListener(OnToggleOneValueChanged);
            _toggleTwo.onValueChanged.RemoveListener(OnToggleTwoValueChanged);
            _toggleThree.onValueChanged.RemoveListener(OnToggleThreeValueChanged);
        }

        private void OnToggleOneValueChanged(bool isOn)
        {
            if (isOn)
            {
                Screen.SetResolution(1920, 1080, true);
            }
        }
        
        private void OnToggleTwoValueChanged(bool isOn)
        {
            if (isOn)
            {
                Screen.SetResolution(2560, 1440, true);
            }
        }
        
        private void OnToggleThreeValueChanged(bool isOn)
        {
            if (isOn)
            {
                Screen.SetResolution(3840, 2160, true);
            }
        }
    }
}
