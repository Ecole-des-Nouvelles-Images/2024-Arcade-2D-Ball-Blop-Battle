using UnityEngine;

namespace Hugo.Prototype.Scripts.Sounds
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager instance;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
