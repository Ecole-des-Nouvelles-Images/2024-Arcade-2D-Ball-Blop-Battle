using UnityEngine;

namespace Hugo.Refacto.Scripts
{
    public class NewLaserHandler : MonoBehaviour
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
                NewBlopController newBlopController = hitPlayer.transform.gameObject.GetComponent<NewBlopController>();
                newBlopController.Die();
                NewMatchManager.Instance.Foul(newBlopController.PlayerId);
            }
            
            // DEBUG
            Debug.DrawRay(transform.position, Vector3.down * _rayLaserLength, Color.red);
        }
    }
}
