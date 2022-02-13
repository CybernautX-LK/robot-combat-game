using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

namespace CybernautX
{
    public class UIManager : MonoSingleton<UIManager>
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

        [SerializeField]
        private GameManagerEvents gameManagerEvents;

        [SerializeField]
        private UIManagerEvents uiManagerEvents;

        [SerializeField]
        private TextMeshProUGUI messageDisplay;

        [SerializeField]
        private float messageDuration;

        [SerializeField]
        private Ease messageEase;

        private MainMenuController mainMenuController;

        private HUDController hudController;

        private void Awake()
        {
            Instance = this;

            if (uiManagerEvents != null)
            {
                uiManagerEvents.OnShowMessageEvent += ShowMessage;
                uiManagerEvents.OnShowMessageWithSettingsEvent += ShowMessage;
                uiManagerEvents.OnUpdateTimerEvent += OnUpdateTimer;
            }

            GameManager.OnGameStartedEvent += OnGameStarted;

            MainMenuController.OnAwakeEvent += OnMainMenuControllerAwake;
            HUDController.OnAwakeEvent += OnHUDControllerAwake;

            if (messageDisplay != null)
                messageDisplay.text = "";
        }

        private void OnHUDControllerAwake(HUDController controller) => hudController = controller;

        private void OnMainMenuControllerAwake(MainMenuController controller) => mainMenuController = controller;

        private void OnDestroy()
        {
            if (uiManagerEvents != null)
            {
                uiManagerEvents.OnShowMessageEvent -= ShowMessage;
                uiManagerEvents.OnShowMessageWithSettingsEvent -= ShowMessage;
                uiManagerEvents.OnUpdateTimerEvent -= OnUpdateTimer;
            }

            GameManager.OnGameStartedEvent -= OnGameStarted;
        }

        public void ShowMessage(string message)
        {
            ShowMessageSettings settings = new ShowMessageSettings(message, messageDuration);
            ShowMessage(settings);
        }

        public void ShowMessage(ShowMessageSettings settings)
        {
            if (messageDisplay == null) return;

            messageDisplay.text = settings.message;
            //messageDisplay.DOText(message, messageDuration).SetEase(messageEase);
            messageDisplay.DOFade(0.0f, settings.duration * 0.5f).SetEase(messageEase).From(1.0f).SetDelay(settings.duration * 0.5f);
            //messageDisplay.DOScale(0.0f, messageDuration).From(1.0f).SetEase(messageEase);
        }

        private void OnUpdateTimer(float time) => hudController?.UpdateTimer(time);

        private void OnGameStarted()
        {
            if (hudController != null)
                hudController.Enable();

            if (mainMenuController != null)
                mainMenuController.CloseAllMenus();
        }
    }
}

