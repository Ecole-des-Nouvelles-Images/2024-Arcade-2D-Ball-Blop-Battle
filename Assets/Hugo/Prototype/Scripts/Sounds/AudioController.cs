using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UIElements;

namespace Hugo.Prototype.Scripts.Sounds
{
    public class AudioController : MonoBehaviour
    {
        [Header("Audio Elements")]
        [SerializeField] private AudioMixer _audioMixer;
        [SerializeField] private Slider _masterSlider;
        [SerializeField] private Slider _musicSlider;
        [SerializeField] private Slider _ambianceSlider;
        [SerializeField] private Slider _sfxSlider;
        
        public void ChangeMasterVolume()
        {
            _audioMixer.SetFloat("MasterVolume", Mathf.Log10(_masterSlider.value) * 20);
        }
        
        public void ChangeMusicVolume()
        {
            _audioMixer.SetFloat("MusicVolume", Mathf.Log10(_musicSlider.value) * 20);
        }
        
        public void ChangeAmbianceVolume()
        {
            _audioMixer.SetFloat("AmbianceVolume", Mathf.Log10(_ambianceSlider.value) * 20);
        }
        
        public void ChangeSfxVolume()
        {
            _audioMixer.SetFloat("SFXVolume", Mathf.Log10(_masterSlider.value) * 20);
        }
    }
}