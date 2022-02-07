using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Sirenix.OdinInspector;

public class UIManager : MonoBehaviour
{
    [System.Serializable]
    public class Menu
    {
        public string menuName = "Insert Menu Name";
        public GameObject gameObject;
    }

    [BoxGroup("Settings")]
    [SerializeField]
    private string defaultMenu = "";

    [BoxGroup("Settings")]
    [SerializeField]
    private bool hideMainMenuButton = true;

    [BoxGroup("References")]
    [SerializeField]
    private GameObject mainMenuButton = null;
    [BoxGroup("References")]
    public GameObject continueButton;

    [BoxGroup("References")]
    [SerializeField]
    private List<Menu> menus = new List<Menu>();

    [BoxGroup("Selectables")]
    [SerializeField]
    private Toggle fullscreenToggle = null;

    [BoxGroup("Selectables")]
    [SerializeField]
    private Toggle windowedToggle = null;


    [BoxGroup("Debug")]
    [ReadOnly]
    [SerializeField]
    private List<Menu> activeMenus = new List<Menu>();

    public UnityEvent OnMainMenuActivation = new UnityEvent();
    public UnityEvent OnMainMenuDeactivation = new UnityEvent();

    private void Awake()
    {
        if (fullscreenToggle != null)
            fullscreenToggle.onValueChanged.AddListener(OnFullscreenToggleChanged);

        if (windowedToggle != null)
            windowedToggle.onValueChanged.AddListener(OnWindowedToggleChanged);
    }

    private void Start()
    {
        OpenMenuSingle(defaultMenu);
    }

    public void OpenMenuSingle(string menuName = "") => OpenMenu(menuName);

    public void OpenMenuAdditive(string menuName = "") => OpenMenu(menuName, true);

    public void OpenMenu(string menuName = "", bool additive = false)
    {
        //if (mainUI != null)
        //    mainUI.SetActive(true);

        if (menus.Count > 0 && menus[0].gameObject != null)
        {
            Menu menuToOpen = (menuName == "") ? menus[0] : GetMenuByName(menuName);

            if (menuToOpen != null)
            {
                if (!additive)
                    CloseAllMenus();

                if (mainMenuButton != null && hideMainMenuButton && mainMenuButton.activeSelf)
                    mainMenuButton.SetActive(false);

                menuToOpen.gameObject.SetActive(true);

                RegisterMenu(menuToOpen);
            }
        }

        OnMainMenuActivation?.Invoke();
    }

    public void CloseMenu(string menuName = "")
    {
        Menu menuToClose = GetMenuByName(menuName);

        if (menuToClose != null)
        {
            menuToClose.gameObject.SetActive(false);
            UnregisterMenu(menuToClose);
        }

        if (activeMenus.Count < 1)
        {
            if (mainMenuButton != null && !hideMainMenuButton && !mainMenuButton.activeSelf)
                mainMenuButton.SetActive(true);

            OnMainMenuDeactivation?.Invoke();
        }

    }

    public void CloseAllMenus()
    {
        foreach (Menu menu in menus)
        {
            menu.gameObject.SetActive(false);
            UnregisterMenu(menu);
        }

        if (mainMenuButton != null && !hideMainMenuButton && !mainMenuButton.activeSelf)
        {
            mainMenuButton.SetActive(true);
        }

        OnMainMenuDeactivation?.Invoke();
    }

    private Menu GetMenuByName(string menuName)
    {
        foreach (Menu menu in menus)
        {
            if (menu.menuName == menuName)
                return menu;
        }

        Debug.LogWarning("[UIManager]: Couldn't find a menu with the name " + menuName);
        return null;
    }

    private void RegisterMenu(Menu menu)
    {
        if (!activeMenus.Contains(menu))
            activeMenus.Add(menu);
    }

    private void UnregisterMenu(Menu menu)
    {
        if (activeMenus.Contains(menu))
            activeMenus.Remove(menu);
    }

    private void OnFullscreenToggleChanged(bool value)
    {
        if (value && Screen.fullScreenMode != FullScreenMode.ExclusiveFullScreen)
        {
            Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
            Debug.Log($"{this.name}: Set ScreenMode to fullscreen.");
        }
    }

    private void OnWindowedToggleChanged(bool value)
    {
        if (value && Screen.fullScreenMode != FullScreenMode.Windowed)
        {
            Screen.fullScreenMode = FullScreenMode.Windowed;
            Debug.Log($"{this.name}: Set ScreenMode to windowed.");
        }
    }
}
