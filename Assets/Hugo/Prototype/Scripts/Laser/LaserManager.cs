using Hugo.Prototype.Scripts.Player;
using UnityEngine;

namespace Hugo.Prototype.Scripts.Laser
{
    public class LaserManager : MonoBehaviour
    {
        [Header("Laser Settings")]
        [SerializeField] private float _rayLaserLength;
        [SerializeField] private LayerMask _playerLayer;
        
        private void Update()
        {
            // Raycast _laserHit
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector3.down, _rayLaserLength, _playerLayer);

            if (hit.collider)
            {
                //Debug.Log(" hit player ");
                GameObject playerChildren = hit.collider.gameObject;
                playerChildren.GetComponentInParent<PlayerController>().PlayerDie();
            }
            
            // Display Laser RayCast
            Debug.DrawRay(transform.position, Vector3.down * _rayLaserLength, Color.red);
        }
    }
}
