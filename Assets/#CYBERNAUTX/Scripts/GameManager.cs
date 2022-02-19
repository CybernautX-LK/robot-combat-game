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
        private int pointsToWin = 3;

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

        [BoxGroup("Messages")]
        [SerializeField]
        private string gameStartedMessage = "Game Starts...";

        [BoxGroup("Messages")]
        [SerializeField]
        private string roundStartedMessage = "Fight!";

        [BoxGroup("Messages")]
        [SerializeField]
        private string suddenDeathMessage = "Sudden Death!";

        private Player currentWinner;
        private float currentRoundTime;
        private Coroutine currentGameCoroutine;

        private bool suddenDeathMode = false;

        public static UnityAction OnGameStartedEvent;
        public static UnityAction OnGameRoundInitializedEvent;
        public static UnityAction OnGameRoundStartedEvent;
        public static UnityAction OnGameRoundCompletedEvent;
        public static UnityAction OnGameCompletedEvent;

        private void Awake()
        {
            Instance = this;

            if (gameManagerEvents != null)
            {
                gameManagerEvents.OnGameStartRequest += StartGame;
                gameManagerEvents.OnGameRoundUpdateRequest += UpdateGameRounds;
                gameManagerEvents.OnReturnToMainMenuRequest += ReturnToMainMenu;
                gameManagerEvents.OnExitGameRequest += ExitGame;
            }
        }

        private void OnDestroy()
        {
            if (gameManagerEvents != null)
            {
                gameManagerEvents.OnGameStartRequest -= StartGame;
                gameManagerEvents.OnGameRoundUpdateRequest -= UpdateGameRounds;
                gameManagerEvents.OnReturnToMainMenuRequest -= ReturnToMainMenu;
                gameManagerEvents.OnExitGameRequest += ExitGame;
            }
        }

        private void Start() => LoadingManager.LoadSceneAdditive("01_MainMenu");

        [Button(ButtonSizes.Large)]
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

            ResetAllPlayers();

            while (!PlayerHasWon())
            {
                yield return StartCoroutine(ResetArenaScene());

                IEnumerator ResetArenaScene()
                {
                    if (LoadingManager.loadedScenes.Contains(LoadingManager.GetSceneByName("02_Arena")))
                    {
                        yield return LoadingManager.UnloadScene("02_Arena");
                    }

                    Debug.Log($"{typeof(GameManager).Name}: Loading Arena Started");

                    yield return LoadingManager.LoadSceneAdditiveAsync("02_Arena");

                    Debug.Log($"{typeof(GameManager).Name}: Loading Arena Complete");
                }

                yield return StartCoroutine(StartGameRound());

                // Game Round Coroutine
                IEnumerator StartGameRound()
                {
                    InitializeGameRound();

                    uiManagerEvents?.ShowMessageWithSettings(new UIManager.ShowMessageSettings(suddenDeathMode ? suddenDeathMessage : gameStartedMessage, roundDelay));

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

                    Debug.Log($"{typeof(GameManager).Name}: Round Started");

                    OnGameRoundStartedEvent?.Invoke();

                    uiManagerEvents?.ShowMessageWithSettings(new UIManager.ShowMessageSettings(roundStartedMessage, 2.0f));

                    foreach (Player player in players)
                    {
                        player.configuration = new Player.Configuration(true, true, true, true);
                    }

                    while (PlayersAlive().Count > 1 && (suddenDeathMode || !TimeIsUp()))
                    {
                        if (!suddenDeathMode)
                            UpdateTimer();

                        yield return null;
                    }

                    foreach (Player player in players)
                    {
                        player.configuration = new Player.Configuration(false, false, false, false);
                    }

                    if (TimeIsUp() && !suddenDeathMode)
                    {
                        suddenDeathMode = true;

                        uiManagerEvents?.ShowMessageWithSettings(new UIManager.ShowMessageSettings($"No one won the round!", 3.0f));
                        yield return new WaitForSeconds(3.0f);
                    }
                    else
                    {
                        suddenDeathMode = false;

                        Player lastPlayerAlive = PlayersAlive()[0];
                        lastPlayerAlive.SetPoints(lastPlayerAlive.currentPoints + 1);

                        if (!PlayerHasWon())
                        {
                            uiManagerEvents?.ShowMessageWithSettings(new UIManager.ShowMessageSettings($"{(lastPlayerAlive != null ? lastPlayerAlive.name : "No one")} won the round!", 3.0f));
                            yield return new WaitForSeconds(3.0f);
                        }
                    }                                        

                    Debug.Log($"{typeof(GameManager).Name}: Round Complete");

                    OnGameRoundCompletedEvent?.Invoke();
                }
            }

            uiManagerEvents?.ShowMessageWithSettings(new UIManager.ShowMessageSettings($"{currentWinner.name} won the game!", 3.0f));
            yield return new WaitForSeconds(3.0f);

            Debug.Log($"{typeof(GameManager).Name}: Game Complete");

            OnGameCompletedEvent?.Invoke();
        }



        [Button(ButtonSizes.Large)]
        public void ReturnToMainMenu()
        {
            if(LoadingManager.loadedScenes.Contains(LoadingManager.GetSceneByName("02_Arena")))
                LoadingManager.UnloadScene("02_Arena");

            if (!LoadingManager.loadedScenes.Contains(LoadingManager.GetSceneByName("01_MainMenu")))
                LoadingManager.LoadSceneAdditiveAsync("01_MainMenu");
        }

        [Button(ButtonSizes.Large)]
        public void ExitGame()
        {
            Application.Quit();
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

        private bool TimeIsUp()
        {
            bool roundTimeEnded = currentRoundTime < 0.0f;

            return roundTimeEnded;
        }

        private List<Player> PlayersAlive()
        {
            List<Player> playersAlive = new List<Player>();

            foreach (Player player in players)
            {
                if (!player.isDead)
                {
                    playersAlive.Add(player);
                }
            }

            return playersAlive;
        }

        private bool PlayerHasWon()
        {
            foreach (Player player in players)
            {
                if (player.currentPoints >= pointsToWin)
                {
                    currentWinner = player;
                    return true;
                }
            }

            currentWinner = null;
            return false;
        }

        private void UpdateTimer()
        {
            currentRoundTime -= Time.deltaTime;
            uiManagerEvents?.UpdateTimer(currentRoundTime);
        }

        private void InitializeGameRound()
        {
            currentRoundTime = suddenDeathMode ? 0.0f : roundDuration;
            uiManagerEvents?.UpdateTimer(currentRoundTime);

            ResetAllPlayersHealth();

            float receivedDamageMultiplier = suddenDeathMode ? 2.0f : 1.0f;
            SetAllPlayersReceivedDamageMultiplier(receivedDamageMultiplier);

            foreach (Player player in players)
            {
                player.configuration = new Player.Configuration(false, true, false, true);
            }

            OnGameRoundInitializedEvent?.Invoke();
        }

        private void ResetAllPlayers()
        {
            foreach (Player player in players)
            {
                player.Reset();
            }
        }

        private void ResetAllPlayersHealth()
        {
            foreach (Player player in players)
            {
                player.SetHealth(player.maxHealth);
            }
        }

        private void SetAllPlayersReceivedDamageMultiplier(float multiplier)
        {
            foreach (Player player in players)
            {
                player.takeDamageMultiplier = multiplier;
            }
        }

        private void UpdateGameRounds(int amount) => pointsToWin = amount;
    }
}

