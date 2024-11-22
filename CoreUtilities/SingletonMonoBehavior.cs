using UnityEngine;

namespace Utilities
{
    public abstract class SingletonMonoBehavior<T> : MonoBehaviour where T : SingletonMonoBehavior<T>
    {
        [Tooltip("If true, the instance will be destroyed when a new scene is loaded.")]
        [SerializeField] private bool _destroyOnLoad;
    
        private static bool _instanceSet;
        private static T _instance;
        protected bool WillBeDestroyed;
        
        /// <summary>
        /// Will return the instance of the singleton. If the instance is not set, it will try to find it in the scene.
        /// </summary>
        public static T Instance
        {
            get
            {
                if (!_instanceSet)
                {
                    _instance = FindObjectOfType<T>();
                    _instanceSet = _instance != null;
                }
                return _instance;
            }
        }

        protected virtual void Awake()
        {
            if(Instance == this && _instanceSet)
            {
                if(_instance.transform.parent != null)
                {
                    _instance.transform.parent = null;
                }
                if (!_destroyOnLoad) DontDestroyOnLoad(_instance.gameObject);
            }
            else
            {
                Destroy(gameObject);
                WillBeDestroyed = true;
            }
        }
    
        protected virtual void OnDestroy()
        {
            if (Instance != this) return;
            
            _instance = null;
            _instanceSet = false;
        }
    }
}
