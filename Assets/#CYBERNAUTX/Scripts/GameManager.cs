using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;
using System;
using System.Linq;

namespace CybernautX
{
    public class GameManager : MonoSingleton<GameManager>
    {
        [BoxGroup("General")]
        [SerializeField]
        private GameManagerEvents gameManagerEvents;

        [BoxGroup("General")]
        [SerializeField]
        private UIManagerEvents uiManagerEvents;

        [BoxGroup("General")]
        [SerializeField]
        private List<Player> players = new List<Player>();

        [BoxGroup("Settings")]
        [SerializeField]
        [Range(1, 5)]
        private int roundsToWin = 3;

        [BoxGroup("Settings")]
        [SerializeField]
        [Range(0, 5)]
        private float roundDelay = 1;

        [BoxGroup("Settings")]
        [SerializeField]
        [Range(0, 5)]
        private int countdownTimer = 5;

        [BoxGroup("Settings")]
        [SerializeField]
        [Range(10, 300)]
        private int roundDuration = 120;

        [BoxGroup("References")]
        [SerializeField]
        private ItemSelection roundsToWinSelection;

        [BoxGroup("Messages")]
        [SerializeField]
        private string gameStartedMessage = "Game Starts...";

        [BoxGroup("Messages")]
        [SerializeField]
        private string roundStartedMessage = "Fight!";

        [BoxGroup("Messages")]
        [SerializeField]
        private string roundEndedMessage = "Game Over!";

        private float currentRoundTime;
        private Coroutine currentGameCoroutine;

        public static UnityAction OnGameStartedEvent;
        public static UnityAction OnGameRoundStartedEvent;

        private void Awake()
        {
            Instance = this;

            if (gameManagerEvents != null)
            {
                gameManagerEvents.OnGameStartEvent += StartGame;
                gameManagerEvents.OnGameRoundUpdateEvent += OnGameRoundUpdate;
            }
        }

        private void OnDestroy()
        {
            if (gameManagerEvents != null)
            {
                gameManagerEvents.OnGameStartEvent -= StartGame;
                gameManagerEvents.OnGameRoundUpdateEvent -= OnGameRoundUpdate;
            }
        }

        private void Start() => LoadingManager.LoadSceneAdditive("01_MainMenu");

        [Button]
        public void StartGame()
        {
            if (!SettingsValid()) return;

            if (currentGameCoroutine != null)
                StopCoroutine(currentGameCoroutine);

            currentGameCoroutine = StartCoroutine(StartGameCoroutine());
        }

        private IEnumerator StartGameCoroutine()
        {
            OnGameStartedEvent?.Invoke();

            Debug.Log($"{typeof(GameManager).Name}: Loading Arena Started");

            yield return LoadingManager.LoadSceneAdditiveAsync("02_Arena");

            Debug.Log($"{typeof(GameManager).Name}: Loading Arena Complete");
           
            InitializeGame();

            uiManagerEvents?.ShowMessageWithSettings(new UIManager.ShowMessageSettings(gameStartedMessage, roundDelay));

            yield return new WaitForSeconds(roundDelay);

            // Countdown Coroutine
            yield return StartCoroutine(CountdownTimer(countdownTimer));

            IEnumerator CountdownTimer(int seconds)
            {
                Debug.Log($"{typeof(GameManager).Name}: Countdown Started");

                for (int i = seconds; i > 0; i--)
                {
                    uiManagerEvents?.ShowMessage($"{i}");

                    Debug.Log($"{typeof(GameManager).Name}: {i}");
                    yield return new WaitForSeconds(1.0f);
                }

                Debug.Log($"{typeof(GameManager).Name}: Countdown Complete");
            }

            yield return StartCoroutine(StartGameRound());

            // Game Round Coroutine
            IEnumerator StartGameRound()
            {
                Debug.Log($"{typeof(GameManager).Name}: Round Started");

                OnGameRoundStartedEvent?.Invoke();

                uiManagerEvents?.ShowMessageWithSettings(new UIManager.ShowMessageSettings(roundStartedMessage, 2.0f));

                while (!GameOver())
                {
                    // Update Timer
                    currentRoundTime -= Time.deltaTime;
                    uiManagerEvents?.UpdateTimer(currentRoundTime);
                    yield return new WaitForEndOfFrame();
                }

                currentRoundTime = 0.0f;
                uiManagerEvents?.UpdateTimer(currentRoundTime);
                uiManagerEvents?.ShowMessageWithSettings(new UIManager.ShowMessageSettings(roundEndedMessage, 3.0f));

                Debug.Log($"{typeof(GameManager).Name}: Round Complete");
            }

            // Game Over Coroutine
        }

        private bool GameOver()
        {
            bool roundTimeEnded = currentRoundTime < 0.0f;
            return roundTimeEnded;
        }

        private void InitializeGame()
        {
            currentRoundTime = roundDuration;
            uiManagerEvents?.UpdateTimer(currentRoundTime);

            // Spawn Players

            // Set Weapons

            // Update UI
        }



        [Button]
        public void ReturnToMainMenu()
        {
            LoadingManager.UnloadScene("02_Arena");
        }

        private bool SettingsValid()
        {

            bool allPlayersUseDifferentWeapons = true;

            foreach (Player player in players)
            {
                List<Item> currentSelectedWeapons = new List<Item>();

                foreach (ItemSlot slot in player.weaponSlots)
                {
                    if (currentSelectedWeapons.Contains(slot.selectedItem))
                    {
                        allPlayersUseDifferentWeapons = false;
                        Debug.LogWarning($"{typeof(GameManager).Name}: {player.name} is not using different weapons.");
                        break;
                    }

                    currentSelectedWeapons.Add(slot.selectedItem);
                }

            }

            bool settingsValid = allPlayersUseDifferentWeapons;

            return settingsValid;
        }

        private void OnGameRoundUpdate(int amount)
        {
            roundsToWin = amount;
        }
    }
}

