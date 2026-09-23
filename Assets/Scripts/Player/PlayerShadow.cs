using UnityEngine;

namespace Player
{
    public class PlayerShadow : MonoBehaviour
    {
        [Header("Settings")] 
        [SerializeField] private float _worldPosY;
        [SerializeField] private Vector2 _scaleFactorMinMax;

        private Transform _parentTransform;
        private Vector3 _initialScale;

        private void Start()
        {
            _parentTransform = transform.parent;
            _initialScale = transform.localScale;
        }

        private void Update()
        {
            // POS
            transform.position = new Vector3(_parentTransform.position.x, _worldPosY, _parentTransform.position.z);

            // SCALE
            float distance = Mathf.Abs(_parentTransform.position.y - _worldPosY);
            float t = Mathf.Clamp01(distance / 8f);
            float currentScale = Mathf.Lerp(_scaleFactorMinMax.y, _scaleFactorMinMax.x, t);
            transform.localScale = _initialScale * currentScale;
        }
    }
}