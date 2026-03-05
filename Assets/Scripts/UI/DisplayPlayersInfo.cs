using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace UI
{
    public class DisplayPlayersInfo : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private Color _perfectReceptionBaseColor = Color.gray;
        [SerializeField] private Color _perfectReceptionActivatedColor = Color.yellow;
        [SerializeField] private Color _fullPerfectReceptionColor = Color.green;
        
        [Header("References")]
        [SerializeField] private List<Image> _p1PerfectReceptions;
        [SerializeField] private List<Image> _p2PerfectReceptions;

        #region === EVENTS ===

        private void OnEnable()
        {
            EventBus.OnPlayerPerfectReception += PlayerPerfectReception;
        }

        private void PlayerPerfectReception(int playerID, int count)
        {
            bool haveFullPerfectReception = count == _p1PerfectReceptions.Count;
                
            if (playerID == 1)
            {
                for (int i = 0; i < _p1PerfectReceptions.Count; i++)
                {
                    if (haveFullPerfectReception)
                    {
                        _p1PerfectReceptions[i].color = _fullPerfectReceptionColor;
                        continue;
                    }
                        
                    if (i < count)
                    {
                        _p1PerfectReceptions[i].color = _perfectReceptionActivatedColor;
                    }
                    else
                    {
                        _p1PerfectReceptions[i].color = _perfectReceptionBaseColor;
                    }
                }
            }
            else if (playerID == 2)
            {
                for (int i = 0; i < _p2PerfectReceptions.Count; i++)
                {
                    if (haveFullPerfectReception)
                    {
                        _p2PerfectReceptions[i].color = _fullPerfectReceptionColor;
                        continue;
                    }
                        
                    if (i < count)
                    {
                        _p2PerfectReceptions[i].color = _perfectReceptionActivatedColor;
                    }
                    else
                    {
                        _p2PerfectReceptions[i].color = _perfectReceptionBaseColor;
                    }
                }
            }
        }
        
        private void OnDisable()
        {
            EventBus.OnPlayerPerfectReception -= PlayerPerfectReception;
        }

        #endregion
    }
}
