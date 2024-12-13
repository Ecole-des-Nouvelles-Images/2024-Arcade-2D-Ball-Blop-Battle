using UnityEngine;

namespace Hugo.Prototype.Scripts.Arene
{
    public class ImpactSpecialSpikeBackGround : MonoBehaviour
    {
        [Header("Impact Special Spike")]
        [SerializeField] private SpriteRenderer _spriteRendererImpact;
        [SerializeField] private SpriteRenderer _spriteRendererWhite;
        [SerializeField] private float _speedDicrase;

        private void Awake()
        {
            _spriteRendererWhite.color = _spriteRendererImpact.color;
        }

        private void Update()
        {
            if (!Mathf.Approximately(_spriteRendererWhite.color.a, 0))
            {
                var color = _spriteRendererWhite.color;
                color.a -= _speedDicrase * Time.deltaTime;
                _spriteRendererWhite.color = color;
            }
        }
    }
}
