using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Utils.Singletons;

namespace Utils
{
    public class SceneLoaderManager : MonoBehaviourSingletonDontDestroyOnLoad<SceneLoaderManager>
    {
        [Header("Loading Settings")]
        [SerializeField] private float _durationLoading = 1f;
        
        [Header("Fade Settings")]
        [SerializeField] private float _durationFade = 1f;
        [SerializeField] private AnimationCurve _curveFade;
        
        [Header("Logo Settings")]
        [SerializeField] private float _durationMove = 1f;
        [SerializeField] private float _speedRotation = 360f;
        [SerializeField] private float _endPos = -1500f;
        [SerializeField] private AnimationCurve _curveMove;
        
        [Header("References")]
        [SerializeField] private Image _imageBackground;
        [SerializeField] private Image _imageLogo;
        
        private Vector3 _startPos;

        private void Start()
        {
            _startPos = _imageLogo.rectTransform.position;
        }

        private void Update()
        {
            _imageBackground.rectTransform.position = _imageLogo.rectTransform.position;
        }
        
        public void QuitGame()
        {
            Application.Quit();
        }
        
        public void LoadScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }

        public void LoadSceneAnimation(string sceneName)
        {
            StartCoroutine(AnimationCoroutine(sceneName));
        }

        private IEnumerator AnimationCoroutine(string sceneName)
        {
            _imageBackground.DOFade(1f, _durationFade).SetEase(_curveFade);
            
            yield return new WaitForSeconds(_durationFade);
            
            SceneManager.LoadSceneAsync(sceneName);

            _imageLogo.rectTransform.DOMoveX(_endPos, _durationMove)
                .SetEase(_curveMove);

            _imageLogo.rectTransform.DORotate(new Vector3(0f, 0f, _speedRotation), 1f, RotateMode.FastBeyond360)
                .SetEase(Ease.Linear)
                .SetLoops(-1, LoopType.Incremental);
            
            yield return new WaitForSeconds(_durationMove);
            
            ResetState();
        }

        private void ResetState()
        {
            DOTween.KillAll();
            
            _imageLogo.rectTransform.rotation = Quaternion.identity;
            var color = _imageBackground.color;
            color.a = 0f;
            _imageBackground.color = color;
            
            _imageLogo.rectTransform.position = _startPos;
        }
    }
}