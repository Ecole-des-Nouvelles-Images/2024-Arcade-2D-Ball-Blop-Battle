using UnityEngine;
using UnityEngine.EventSystems;

namespace Hugo.Prototype.Scripts.UI
{
    public class UITutorialHandler : MonoBehaviour
    {
        [Header("Buttons")]
        [SerializeField] private GameObject _sautButton;
        [SerializeField] private GameObject _dashButton;
        [SerializeField] private GameObject _receptionParfaiteButton;
        [SerializeField] private GameObject _absorptionButton;
        [SerializeField] private GameObject _blepButton;
        [SerializeField] private GameObject _blopButton;
        [SerializeField] private GameObject _blupButton;
        [SerializeField] private GameObject _blapButton;
        
        [Header("Panels")]
        [SerializeField] private GameObject _sautPanel;
        [SerializeField] private GameObject _dashPanel;
        [SerializeField] private GameObject _receptionParfaitePanel;
        [SerializeField] private GameObject _absorptionPanel;
        [SerializeField] private GameObject _blepPanel;
        [SerializeField] private GameObject _blopPanel;
        [SerializeField] private GameObject _blupPanel;
        [SerializeField] private GameObject _blapPanel;
        
        [Header("Event Systems")]
        [SerializeField] private EventSystem _eventSystem;
        
        private GameObject _currentSelectedButton;

        private void Update()
        {
            _currentSelectedButton = _eventSystem.currentSelectedGameObject;

            if (_currentSelectedButton)
            {
                if (_currentSelectedButton == _sautButton)
                {
                    _sautPanel.SetActive(true);
                    _dashPanel.SetActive(false);
                    _receptionParfaitePanel.SetActive(false);
                    _absorptionPanel.SetActive(false);
                    _blepPanel.SetActive(false);
                    _blopPanel.SetActive(false);
                    _blupPanel.SetActive(false);
                    _blapPanel.SetActive(false);
                }

                if (_currentSelectedButton == _dashButton)
                {
                    _sautPanel.SetActive(false);
                    _dashPanel.SetActive(true);
                    _receptionParfaitePanel.SetActive(false);
                    _absorptionPanel.SetActive(false);
                    _blepPanel.SetActive(false);
                    _blopPanel.SetActive(false);
                    _blupPanel.SetActive(false);
                    _blapPanel.SetActive(false);
                }

                if (_currentSelectedButton == _receptionParfaiteButton)
                {
                    _sautPanel.SetActive(false);
                    _dashPanel.SetActive(false);
                    _receptionParfaitePanel.SetActive(true);
                    _absorptionPanel.SetActive(false);
                    _blepPanel.SetActive(false);
                    _blopPanel.SetActive(false);
                    _blupPanel.SetActive(false);
                    _blapPanel.SetActive(false);
                }

                if (_currentSelectedButton == _absorptionButton)
                {
                    _sautPanel.SetActive(false);
                    _dashPanel.SetActive(false);
                    _receptionParfaitePanel.SetActive(false);
                    _absorptionPanel.SetActive(true);
                    _blepPanel.SetActive(false);
                    _blopPanel.SetActive(false);
                    _blupPanel.SetActive(false);
                    _blapPanel.SetActive(false);
                }

                if (_currentSelectedButton == _blepButton)
                {
                    _sautPanel.SetActive(false);
                    _dashPanel.SetActive(false);
                    _receptionParfaitePanel.SetActive(false);
                    _absorptionPanel.SetActive(false);
                    _blepPanel.SetActive(true);
                    _blopPanel.SetActive(false);
                    _blupPanel.SetActive(false);
                    _blapPanel.SetActive(false);
                }

                if (_currentSelectedButton == _blopButton)
                {
                    _sautPanel.SetActive(false);
                    _dashPanel.SetActive(false);
                    _receptionParfaitePanel.SetActive(false);
                    _absorptionPanel.SetActive(false);
                    _blepPanel.SetActive(false);
                    _blopPanel.SetActive(true);
                    _blupPanel.SetActive(false);
                    _blapPanel.SetActive(false);
                }

                if (_currentSelectedButton == _blupButton)
                {
                    _sautPanel.SetActive(false);
                    _dashPanel.SetActive(false);
                    _receptionParfaitePanel.SetActive(false);
                    _absorptionPanel.SetActive(false);
                    _blepPanel.SetActive(false);
                    _blopPanel.SetActive(false);
                    _blupPanel.SetActive(true);
                    _blapPanel.SetActive(false);
                }

                if (_currentSelectedButton == _blapButton)
                {
                    _sautPanel.SetActive(false);
                    _dashPanel.SetActive(false);
                    _receptionParfaitePanel.SetActive(false);
                    _absorptionPanel.SetActive(false);
                    _blepPanel.SetActive(false);
                    _blopPanel.SetActive(false);
                    _blupPanel.SetActive(false);
                    _blapPanel.SetActive(true);
                }
            }
        }
    }
}
