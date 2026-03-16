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
        [SerializeField] private bool _showLogo = true;
        [SerializeField] private float _durationLoading = 1f;
        
        [Header("Fade Settings")]
        [SerializeField] private float _durationFade = 1f;
        [SerializeField] private AnimationCurve _curveFade;
        
        [Header("Logo Settings")]
        [SerializeField] private float _loopScale = 0.5f;
        [SerializeField] private float _durationScale = 1f;
        [SerializeField] private AnimationCurve _curveScale;
        
        [Header("References")]
        [SerializeField] private Image _imageBackground;
        [SerializeField] private Image _imageLogo;

        private void Start()
        {
            _imageLogo.transform.localScale = Vector3.zero;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                StartCoroutine(AnimationCoroutine(2));
            }
        }

        private IEnumerator AnimationCoroutine(int sceneIndex)
        {
            _imageBackground.DOFade(1f, _durationFade).SetEase(_curveFade);
            
            yield return new WaitForSeconds(_durationFade);
            
            SceneManager.LoadSceneAsync(sceneIndex);

            if (_showLogo)
            {
                _imageLogo.transform.DOScale(1f, _durationScale)
                    .OnComplete(() =>
                    {
                        _imageLogo.transform.DOScale(_loopScale, _durationScale)
                            .SetEase(_curveScale)
                            .SetLoops(-1, LoopType.Yoyo);
                    });

                _imageLogo.transform.DORotate(new Vector3(0f, 0f, 180f), 1f, RotateMode.FastBeyond360)
                    .SetEase(Ease.Linear)
                    .SetLoops(-1, LoopType.Incremental);
            }
            
            yield return new WaitForSeconds(_durationLoading);
            
            _imageBackground.DOFade(0f, _durationFade).SetEase(_curveFade);
            if (_showLogo) _imageLogo.DOFade(0f, _durationFade).SetEase(_curveFade);
        }
    }
}
