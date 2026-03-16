using Player.ScriptableObjects;
using UnityEngine;
using Utils;

namespace Arene
{
    public class ArenaSpecialSpikeVFX : MonoBehaviour
    {
        [Header("VFX")]
        [SerializeField] private ParticleSystem _psWindPlayerOne;
        [SerializeField] private ParticleSystem _psWindPlayerTwo;
        [SerializeField] private GameObject _backgroundImpact;
        [SerializeField] private GameObject _backgroundBlop;
        [SerializeField] private GameObject _backgroundScroll;

        private GameObject _currentBackgroundImpact;
        private GameObject _currentBackgroundBlop;
        private GameObject _currentBackgroundScroll;

        #region ===== EVENTS =====

        private void OnEnable()
        {
            EventBus.OnSpecialSpikeActivated += SpecialSpikeActivated;
            EventBus.OnAbsorbedSpecialSpike += AbsorbedSpecialSpike;
            EventBus.OnPlayerScored += PlayerScored;
            EventBus.OnFoul += DestroyBackground;
        }

        private void DestroyBackground()
        {
            if (_currentBackgroundImpact) Destroy(_currentBackgroundImpact);
            if (_currentBackgroundBlop) Destroy(_currentBackgroundBlop);
            if (_currentBackgroundScroll) Destroy(_currentBackgroundScroll);
        }

        private void PlayerScored(int obj)
        {
            DestroyBackground();
        }

        private void OnDisable()
        {
            EventBus.OnSpecialSpikeActivated -= SpecialSpikeActivated;
            EventBus.OnAbsorbedSpecialSpike -= AbsorbedSpecialSpike;
            EventBus.OnPlayerScored -= PlayerScored;
            EventBus.OnFoul -= DestroyBackground;
        }

        private void SpecialSpikeActivated(int playerId, BlopType blopType)
        {
            DestroyBackground();
            
            if (playerId == 1)
            {
                _psWindPlayerOne.Play();
            }
            else if (playerId == 2)
            {
                _psWindPlayerTwo.Play();
            }
            
            _currentBackgroundImpact = Instantiate(_backgroundImpact, transform.position, Quaternion.identity);
            _currentBackgroundImpact.GetComponent<SpecialSpikeImpact>().Setup(playerId, blopType);
            
            _currentBackgroundBlop = Instantiate(_backgroundBlop, transform.position, Quaternion.identity);
            _currentBackgroundBlop.GetComponent<SpecialSpikeBlop>().Setup(playerId, blopType);
        }
        
        private void AbsorbedSpecialSpike(int playerId, BlopType blopType)
        {
            _currentBackgroundScroll = Instantiate(_backgroundScroll, transform.position, Quaternion.identity);
            _currentBackgroundScroll.GetComponent<SpecialSpikeScroll>().Setup(blopType);

            if (_currentBackgroundImpact) Destroy(_currentBackgroundImpact);
            if (_currentBackgroundBlop) Destroy(_currentBackgroundBlop);
        }

        #endregion
    }
}