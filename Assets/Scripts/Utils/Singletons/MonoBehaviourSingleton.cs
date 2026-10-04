using UnityEngine;

namespace Utils.Singletons
{
    public class MonoBehaviourSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;

        [Header("===== Singleton Settings =====")]
        [SerializeField] private bool _isDontDestroyOnLoad = true;

        public static T Instance
        {
            get
            {
                if (_instance != null) return _instance;

                _instance = FindAnyObjectByType<T>();
                if (_instance != null)
                {
                    ApplyDontDestroyOnLoadIfNeeded(_instance);
                }

                return _instance;
            }
        }

        protected virtual void Awake()
        {
            if (_instance == null)
            {
                _instance = this as T;
                ApplyDontDestroyOnLoadIfNeeded(_instance);
            }
            else if (_instance != this)
            {
                Destroy(gameObject);
            }
        }

        private static void ApplyDontDestroyOnLoadIfNeeded(T instance)
        {
            if (instance is MonoBehaviourSingleton<T> singleton && singleton._isDontDestroyOnLoad)
            {
                if (instance.transform.parent != null)
                {
                    instance.transform.SetParent(null);
                }

                DontDestroyOnLoad(instance.gameObject);
            }
        }
    }
}