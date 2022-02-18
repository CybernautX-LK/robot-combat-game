using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using Sirenix.OdinInspector;

namespace CybernautX
{
    public class UIManager : MonoBehaviour
    {
        public class ShowMessageSettings
        {
            public ShowMessageSettings(string message, float duration)
            {
                this.message = message;
                this.duration = duration;
            }

            public string message;
            public float duration;
        }

        [BoxGroup("General")]
        [SerializeField]
        private GameManagerEvents gameManagerEvents;

        [BoxGroup("General")]
        [SerializeField]
        private UIManagerEvents uiManagerEvents;

        [BoxGroup("Settings")]
        [Range(1.0f, 3.0f)]
        [SerializeField]
        private float defaultMessageDuration;

        [BoxGroup("Settings")]
        [SerializeField]
        private Ease defaultMessageEase;

        [BoxGroup("References")]
        [SerializeField]
        private TextMeshProUGUI messageDisplay;

        private MainMenuController mainMenuController;
        private HUDController hudController;

        private void Awake()
        {
            if (uiManagerEvents != null)
            {
                uiManagerEvents.OnShowMessageEvent += ShowMessage;
                uiManagerEvents.OnShowMessageWithSettingsEvent += ShowMessage;
                uiManagerEvents.OnUpdateTimerEvent += OnUpdateTimer;
            }

            if (gameManagerEvents != null)
            {
                gameManagerEvents.OnReturnToMainMenuRequest += OnReturnToMainMenu;
            }

            GameManager.OnGameStartedEvent += OnGameStarted;
            GameManager.OnGameCompletedEvent += OnGameCompleted;

            MainMenuController.OnAwakeEvent += OnMainMenuControllerAwake;
            HUDController.OnAwakeEvent += OnHUDControllerAwake;

            if (messageDisplay != null)
                messageDisplay.text = "";
        }

        private void OnDestroy()
        {
            if (uiManagerEvents != null)
            {
                uiManagerEvents.OnShowMessageEvent -= ShowMessage;
                uiManagerEvents.OnShowMessageWithSettingsEvent -= ShowMessage;
                uiManagerEvents.OnUpdateTimerEvent -= OnUpdateTimer;
            }

            if (gameManagerEvents != null)
            {
                gameManagerEvents.OnReturnToMainMenuRequest -= OnReturnToMainMenu;
            }

            GameManager.OnGameStartedEvent -= OnGameStarted;
            GameManager.OnGameCompletedEvent -= OnGameCompleted;
        }

        public void ShowMessage(string message)
        {
            ShowMessageSettings settings = new ShowMessageSettings(message, defaultMessageDuration);
            ShowMessage(settings);
        }

        public void ShowMessage(ShowMessageSettings settings)
        {
            if (messageDisplay == null) return;

            if (DOTween.IsTweening(messageDisplay))
                DOTween.Kill(messageDisplay);

            messageDisplay.text = settings.message;
            messageDisplay.DOFade(0.0f, settings.duration * 0.5f).SetEase(defaultMessageEase).From(1.0f).SetDelay(settings.duration * 0.5f);
        }

        private void OnUpdateTimer(float time) => hudController?.UpdateTimer(time);

        private void OnGameStarted()
        {
            if (hudController != null)
                hudController.Enable();

            if (mainMenuController != null)
                mainMenuController.CloseAllMenus();
        }

        private void OnHUDControllerAwake(HUDController controller) => hudController = controller;

        private void OnMainMenuControllerAwake(MainMenuController controller) => mainMenuController = controller;


        private void OnGameCompleted()
        {
            if (mainMenuController != null)
                mainMenuController.OpenMenuSingle("GameOverMenu");
        }

        private void OnReturnToMainMenu()
        {
            if (mainMenuController != null)
                mainMenuController.OpenMenuSingle("MainMenu");
        }
    }
}

