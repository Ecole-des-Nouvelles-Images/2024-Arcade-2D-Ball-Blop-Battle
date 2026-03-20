using System.Collections;
using UnityEngine;
using Utils;

namespace UI
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
            StartCoroutine(DisplayCoroutine(delay));
        }
        
        private void MatchOver(int obj)
        {
            _panelMatchOver.SetActive(true);
        }

        #endregion

        private IEnumerator DisplayCoroutine(float delay)
        {
            _panelNewSet.SetActive(true);
            
            yield return new WaitForSeconds(delay * 0.8f);
            
            _panelNewSet.SetActive(false);
        }
    }
}
