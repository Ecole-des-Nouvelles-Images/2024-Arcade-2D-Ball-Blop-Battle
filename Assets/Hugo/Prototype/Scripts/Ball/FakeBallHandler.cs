using UnityEngine;

namespace Hugo.Prototype.Scripts.Ball
{
    public class FakeBallHandler : MonoBehaviour
    {
        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                Destroy(gameObject);
            }
        }
    }
}
