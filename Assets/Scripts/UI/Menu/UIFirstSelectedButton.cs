using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Menu
{
    public class UIFirstSelectedButton : MonoBehaviour
    {
        [Header("First Selected Button")]
        [SerializeField] private GameObject _firstSelectedButton;

        private void OnEnable()
        {
            StartCoroutine(SelectFirstDelayed());
        }

        private IEnumerator SelectFirstDelayed()
        {
            yield return null;
            if (_firstSelectedButton && EventSystem.current)
            {
                EventSystem.current.SetSelectedGameObject(_firstSelectedButton);
            }
        }
    }
}