using System.Collections.Generic;
using Managers;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Branches.Hugo.OldScripts.UI
{
    public class UICanvasNewSetHandler : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private TextMeshProUGUI _textScoreSet;
        [SerializeField] private List<GameObject> _sponsored;

        private void OnEnable()
        {
            foreach (GameObject panel in _sponsored)
            {
                panel.SetActive(false);
            }
            
            _textScoreSet.text = MatchManager.Instance.PlayerOneSetCount + "  -  " + MatchManager.Instance.PlayerTwoSetCount;
            
            int randomNumber = Random.Range(0, _sponsored.Count);
            _sponsored[randomNumber].SetActive(true);
        }
    }
}
