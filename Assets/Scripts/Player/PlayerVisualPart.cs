using Player.ScriptableObjects;
using UnityEngine;

namespace Player
{
    public class PlayerVisualPart : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private PlayerController _playerController;

        #region ===== EVENTS =====

        private void OnEnable()
        {
            _playerController.PlayerEvents.OnAbsorb += PlayerEventsRotateSprite;
            _playerController.PlayerEvents.OnDrawn += PlayerEventsRotateSprite;
        }

        private void OnDisable()
        {
            _playerController.PlayerEvents.OnAbsorb -= PlayerEventsRotateSprite;
            _playerController.PlayerEvents.OnDrawn -= PlayerEventsRotateSprite;
        }
        
        private void PlayerEventsRotateSprite(Blop blop, Transform ballTransform)
        {
            Vector3 dir = ballTransform.position - transform.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

            if (Mathf.Abs(transform.eulerAngles.y - 180f) < 0.1f)
            {
                angle = 180f - angle; 
            }

            transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, angle);
    
            CancelInvoke(nameof(ResetRotation));
            Invoke(nameof(ResetRotation), 0.15f);
        }

        #endregion

        private void ResetRotation()
        {
            transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);
        }
    }
}
