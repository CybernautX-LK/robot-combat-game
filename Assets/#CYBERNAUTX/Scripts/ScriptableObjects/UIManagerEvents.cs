using UnityEngine;
using UnityEngine.Events;

namespace CybernautX
{
    [CreateAssetMenu(fileName = "UIManagerEvents", menuName = "CybernautX/Events/UI Manager Events")]
    public class UIManagerEvents : ScriptableObject
    {
        public UnityAction<string> OnShowMessageEvent;
        public UnityAction<UIManager.ShowMessageSettings> OnShowMessageWithSettingsEvent;
        public UnityAction<float> OnUpdateTimerEvent;

        public void ShowMessage(string message) => OnShowMessageEvent?.Invoke(message);

        public void ShowMessageWithSettings(UIManager.ShowMessageSettings settings) => OnShowMessageWithSettingsEvent?.Invoke(settings);

        public void UpdateTimer(float time) => OnUpdateTimerEvent?.Invoke(time);
    }
}
