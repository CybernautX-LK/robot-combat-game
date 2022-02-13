using UnityEngine;
using UnityEngine.Events;

namespace CybernautX 
{
    [CreateAssetMenu(fileName = "GameManagerEvents", menuName = "CybernautX/Events/Game Manager Events")]
    public class GameManagerEvents : ScriptableObject
    {
        public UnityAction OnGameStartEvent;
        public UnityAction<int> OnGameRoundUpdateEvent;

        public void StartGame() => OnGameStartEvent?.Invoke();
        public void SetGameRounds(int amount) => OnGameRoundUpdateEvent?.Invoke(amount);
    }
}

