using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Menu
{
    public class UITutorialHandler : MonoBehaviour
    {
        [Header("Tutorial Configuration")]
        [SerializeField] private List<TutorialLink> _tutorialLinks;
        
        private GameObject _lastSelectedButton;

        private void Start()
        {
            // Optionnel : S'assurer qu'au moins un panel est actif au début 
            // ou tout cacher selon ton besoin.
            UpdatePanels(EventSystem.current.currentSelectedGameObject);
        }

        private void Update()
        {
            GameObject currentSelected = EventSystem.current.currentSelectedGameObject;

            if (currentSelected != _lastSelectedButton)
            {
                _lastSelectedButton = currentSelected;
                UpdatePanels(currentSelected);
            }
        }

        private void UpdatePanels(GameObject selectedButton)
        {
            if (!selectedButton) return;

            foreach (var link in _tutorialLinks)
            {
                if (link.Panel)
                {
                    link.Panel.SetActive(link.Button == selectedButton);
                }
            }
        }
    }
    
    [Serializable]
    public struct TutorialLink
    {
        public GameObject Button;
        public GameObject Panel;
    }
}