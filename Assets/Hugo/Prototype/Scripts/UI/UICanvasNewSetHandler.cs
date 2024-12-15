using System.Collections.Generic;
using Hugo.Prototype.Scripts.Game;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Hugo.Prototype.Scripts.UI
{
    public class UICanvasNewSetHandler : MonoBehaviour
    {
        private int _countSets;
        
        [Header("References")]
        [SerializeField] private MatchManager _matchManager;
        [SerializeField] private TextMeshProUGUI _textCurrentSet;
        [SerializeField] private TextMeshProUGUI _textScoreSet;
        [SerializeField] private List<GameObject> _sponsored;
        
        private void OnEnable()
        {
            foreach (GameObject panel in _sponsored)
            {
                panel.SetActive(false);
            }

            _countSets++;

            int randomNumber = Random.Range(0, 5);
            switch (_countSets)
            {
                case 1:
                    _textCurrentSet.text = " Premier Set ";
                    _textScoreSet.text = _matchManager.SetScorePlayerOne + " - " + _matchManager.SetScorePlayerTwo;
                    _sponsored[randomNumber].SetActive(true);
                    break;
                case 2:
                    _textCurrentSet.text = "Deuxième Set";
                    _textScoreSet.text = _matchManager.SetScorePlayerOne + " - " + _matchManager.SetScorePlayerTwo;
                    _sponsored[randomNumber].SetActive(true);
                    break;
                case 3:
                    _textCurrentSet.text = "Troisième Set";
                    _textScoreSet.text = _matchManager.SetScorePlayerOne + " - " + _matchManager.SetScorePlayerTwo;
                    _sponsored[randomNumber].SetActive(true);
                    break;
                case 4:
                    _textCurrentSet.text = "Quatrième Set";
                    _textScoreSet.text = _matchManager.SetScorePlayerOne + " - " + _matchManager.SetScorePlayerTwo;
                    _sponsored[randomNumber].SetActive(true);
                    break;
                case 5:
                    _textCurrentSet.text = "Cinquième Set";
                    _textScoreSet.text = _matchManager.SetScorePlayerOne + " - " + _matchManager.SetScorePlayerTwo;
                    _sponsored[randomNumber].SetActive(true);
                    break;
            }
        }
    }
}
