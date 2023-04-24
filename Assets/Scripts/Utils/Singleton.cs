using UnityEngine;

namespace Monster.Utils
{
    public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
    {
        public static T Instance { get; protected set; }

        protected virtual void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            else if (Instance == null)
            {
                Instance = this as T;
                DontDestroyOnLoad(gameObject);
            }
        }
    }
}