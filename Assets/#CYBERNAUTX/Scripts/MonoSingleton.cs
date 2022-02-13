using UnityEngine;
using System;
using Sirenix.OdinInspector;

namespace CybernautX
{
    public class MonoSingleton<T> : SerializedMonoBehaviour
    {
        public bool isInitialized { get; private set; }

        private static T _instance;
        public static T Instance
        {
            get
            {
                if (_instance != null)
                {
                    return _instance;
                }
                else
                {
                    string message = typeof(T).Name + " is not attached to a gameObject or the gameObject is not active.\nMake also sure to set 'Instance = this;' in your Awake() function!";

                    Debug.LogError(message);
                    return default(T);
                    //throw new NullReferenceException(message);
                }
            }
            protected set
            {
                _instance = value;
            }
        }

        public static bool IsInitialized
        {
            get { return _instance != null; }
        }

        public virtual void Init()
        {
            isInitialized = true;
        }

        public virtual void Reset()
        {
            isInitialized = false;
        }

        protected virtual void OnApplicationQuit()
        {
            _instance = default(T);
        }
    }

}
