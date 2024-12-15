using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace Hugo.Prototype.Scripts.UI
{
    public class UIVolumeHandler : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private AudioMixer _audioMixer;
        [SerializeField] private Slider _sliderMaster;
        [SerializeField] private Slider _sliderMusic;
        [SerializeField] private Slider _sliderAmbiance;
        [SerializeField] private Slider _sliderSFX;

        private string MasterVolume = "MasterVolume";
        private string MusicVolume = "MusicVolume";
        private string AmbianceVolume = "AmbianceVolume";
        private string SFXVolume = "SFXVolume";

        private void Awake()
        {
            _sliderMaster.onValueChanged.AddListener(SetMasterVolume);
            _sliderMusic.onValueChanged.AddListener(SetMusicVolume);
            _sliderAmbiance.onValueChanged.AddListener(SetAmbianceVolume);
            _sliderSFX.onValueChanged.AddListener(SetVFXVolume);
        }

        private void OnEnable()
        {
            if (_audioMixer.GetFloat(MasterVolume, out float masterVolume))
            {
                float normalizedMasterVolume = Mathf.InverseLerp(-40f, 0f, masterVolume);
                _sliderMaster.value = normalizedMasterVolume;
            }
            
            if (_audioMixer.GetFloat(MusicVolume, out float musicVolume))
            {
                float normalizedMusicVolume = Mathf.InverseLerp(-40f, 0f, musicVolume);
                _sliderMusic.value = normalizedMusicVolume;
            }
            
            if (_audioMixer.GetFloat(AmbianceVolume, out float ambianceVolume))
            {
                float normalizedAmbianceVolume = Mathf.InverseLerp(-40f, 0f, ambianceVolume);
                _sliderAmbiance.value = normalizedAmbianceVolume;
            }
            
            if (_audioMixer.GetFloat(SFXVolume, out float sfxVolume))
            {
                float normalizedSfxVolume = Mathf.InverseLerp(-40f, 0f, sfxVolume);
                _sliderSFX.value = normalizedSfxVolume;
            }
        }

        private void SetMasterVolume(float value)
        {
            _audioMixer.SetFloat(MasterVolume, Mathf.Lerp(-40f, 0f, value));
            if (_sliderMaster.value == 0)
            {
                _audioMixer.SetFloat(MasterVolume, -80f);
            }
        }
        
        private void SetMusicVolume(float value)
        {
            _audioMixer.SetFloat(MusicVolume, Mathf.Lerp(-40f, 0f, value));
            if (_sliderMusic.value == 0)
            {
                _audioMixer.SetFloat(MusicVolume, -80f);
            }
        }
        
        private void SetAmbianceVolume(float value)
        {
            _audioMixer.SetFloat(AmbianceVolume, Mathf.Lerp(-40f, 0f, value));
            if (_sliderAmbiance.value == 0)
            {
                _audioMixer.SetFloat(AmbianceVolume, -80f);
            }
        }
        
        private void SetVFXVolume(float value)
        {
            _audioMixer.SetFloat(SFXVolume, Mathf.Lerp(-40f, 0f, value));
            if (_sliderSFX.value == 0)
            {
                _audioMixer.SetFloat(SFXVolume, -80f);
            }
        }
    }
}
