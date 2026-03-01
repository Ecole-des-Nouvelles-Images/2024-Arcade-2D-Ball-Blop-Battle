using Managers;
using Player;
using UnityEngine;

namespace Laser
{
    public class LaserHandler : MonoBehaviour
    {
        [Header("Raycast Settings")]
        [SerializeField] private float _rayLaserLength;
        [SerializeField] private LayerMask _playerLayer;

        private void Update()
        {
            RaycastHit2D hitPlayer = Physics2D.Raycast(transform.position, Vector3.down,
                _rayLaserLength, _playerLayer);

            if (hitPlayer)
            {
                Debug.Log("Hit Player");
                hitPlayer.collider.gameObject.SetActive(false);
                PlayerController playerController = hitPlayer.transform.gameObject.GetComponent<PlayerController>();
                playerController.Die();
                MatchManager.Instance.Foul(playerController.PlayerId);
            }
            
            // DEBUG
            Debug.DrawRay(transform.position, Vector3.down * _rayLaserLength, Color.red);
        }
    }
}
