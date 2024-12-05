using UnityEngine;
using UnityEngine.EventSystems;

namespace Hugo.Prototype.Scripts.UI
{
    public class UIFirstSelectedButton : MonoBehaviour
    {
        [SerializeField] private GameObject _firstSelectedButton;

        private void OnEnable()
        {
            EventSystem.current.SetSelectedGameObject(_firstSelectedButton);
        }
    }
}
