using UnityEngine;
using UnityEngine.Events;

namespace CybernautX
{
    [CreateAssetMenu(fileName = "GameManagerEvents", menuName = "CybernautX/Events/Game Manager Events")]
    public class GameManagerEvents : ScriptableObject
    {
        public UnityAction OnGameStartRequest;
        public UnityAction OnReturnToMainMenuRequest;
        public UnityAction<int> OnGameRoundUpdateRequest;
        public UnityAction OnExitGameRequest;

        public void StartGame() => OnGameStartRequest?.Invoke();
        public void SetGameRounds(int amount) => OnGameRoundUpdateRequest?.Invoke(amount);

        public void ReturnToMainMenu() => OnReturnToMainMenuRequest?.Invoke();

        public void ExitGame() => OnExitGameRequest?.Invoke();
    }
}

