using UnityEngine;
using UnityEngine.Events;


namespace CybernautX
{
    public abstract class CustomUnityEvents
    {
        [System.Serializable]
        public class IntEvent : UnityEvent<int> { };

        [System.Serializable]
        public class FloatEvent : UnityEvent<float> { };

        [System.Serializable]
        public class StringEvent : UnityEvent<string> { };

        [System.Serializable]
        public class ItemEvent : UnityEvent<Item> { };
    }
}

