using System.Collections;
using UnityEngine;
using Utils;

namespace UI.InGame
{
    public class DisplayMatchAdvancementInfo : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameObject _panelNewSet;
        [SerializeField] private GameObject _panelMatchOver;

        #region ===== EVENTS =====

        private void OnEnable()
        {
            EventBus.OnSetIsOver += SetIsOver;
            EventBus.OnMatchOver += MatchOver;
        }

        private void OnDisable()
        {
            EventBus.OnSetIsOver -= SetIsOver;
            EventBus.OnMatchOver -= MatchOver;
        }
        
        private void SetIsOver(float delay)
        {
            StartCoroutine(CoroutineDisplaySetOver(delay));
        }
        
        private void MatchOver(int obj)
        {
            StartCoroutine(CoroutineDisplayMatchOver(3f));
        }

        #endregion

        private IEnumerator CoroutineDisplaySetOver(float delay)
        {
            _panelNewSet.SetActive(true);
            
            yield return new WaitForSeconds(delay * 0.8f);
            
            _panelNewSet.SetActive(false);
        }
        
        private IEnumerator CoroutineDisplayMatchOver(float delay)
        {
            yield return new WaitForSeconds(delay);
            
            _panelMatchOver.SetActive(true);
        }
    }
}
