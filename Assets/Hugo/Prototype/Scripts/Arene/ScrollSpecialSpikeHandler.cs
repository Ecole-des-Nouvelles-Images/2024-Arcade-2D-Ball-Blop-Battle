using Hugo.Prototype.Scripts.Player;
using UnityEngine;

namespace Hugo.Prototype.Scripts.Arene
{
    public class ScrollSpecialSpikeHandler : MonoBehaviour
    {
        private bool _isSpawn;
        
        [Header("Settings")]
        [SerializeField] private float _speed; // Vitesse d'augmentation
        [SerializeField] private GameObject _scrollBackGround; // Vitesse d'augmentation

        private void Awake()
        {
            PlayerController.ScrollBackgroundObjects.Add(gameObject);
        }

        private void Update()
        {
            // Augmente la position sur l'axe Y à chaque frame
            transform.position += Vector3.up * (_speed * Time.deltaTime);

            if (transform.position.y >= 8 && !_isSpawn)
            {
                _isSpawn = true;
                Instantiate(_scrollBackGround, new Vector2(0, -21.98f), Quaternion.identity);
            }
            
            if (transform.position.y > 20)
            {
                Destroy(gameObject);
            }
        }
    }
}
