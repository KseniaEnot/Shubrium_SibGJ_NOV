using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [Header("Title Screen")]
    [SerializeField] private GameObject _titleScreenWindow;
    [SerializeField] private Button _titleScreenButtonStart;
    [Header("Main Menu")]
    [SerializeField] private GameObject _mainMenuWindow;
    [SerializeField] private Button _mainMenuButtonStartGame;
    [SerializeField] private Button _mainMenuButtonSettings;
    [SerializeField] private Button _mainMenuButtonQuitGame;
    [Header("Settings Menu")]
    [SerializeField] private GameObject _settingMenuWindow;
    [SerializeField] private Button _settingsMenuButtonBack;

    private void Awake()
    {
        _titleScreenButtonStart.Select();
    }

    private void OnEnable()
    {
        // title screen
        _titleScreenButtonStart.onClick.AddListener(OnTitleScreenButtonStartPressed);
        // main menu
        _mainMenuButtonStartGame.onClick.AddListener(OnMainMenuButtonStartGamePressed);
        _mainMenuButtonSettings.onClick.AddListener(OnMainMenuButtonSettingsPressed);
        _mainMenuButtonQuitGame.onClick.AddListener(OnMainMenuButtonQuitGamePressed);
        // settings
        _settingsMenuButtonBack.onClick.AddListener(OnSettingsMenuButtonBackPressed);
    }

    private void OnDisable()
    {
        // title screen
        _titleScreenButtonStart.onClick.RemoveListener(OnTitleScreenButtonStartPressed);
        // main menu
        _mainMenuButtonStartGame.onClick.RemoveListener(OnMainMenuButtonStartGamePressed);
        _mainMenuButtonSettings.onClick.RemoveListener(OnMainMenuButtonSettingsPressed);
        _mainMenuButtonQuitGame.onClick.RemoveListener(OnMainMenuButtonQuitGamePressed);
        // settings
        _settingsMenuButtonBack.onClick.RemoveListener(OnSettingsMenuButtonBackPressed);
    }

    private void OnTitleScreenButtonStartPressed()
    {
        _titleScreenWindow.SetActive(false);
        _mainMenuWindow.SetActive(true);
        _mainMenuButtonStartGame.Select();
        // код для инициализации
    }

    private void OnMainMenuButtonStartGamePressed()
    {
        // create new game
        SceneManager.LoadScene(1, LoadSceneMode.Single);// временно
        _mainMenuWindow.SetActive(false);
    }

    private void OnMainMenuButtonSettingsPressed()
    {
        _mainMenuWindow.SetActive(false);
        _settingMenuWindow.SetActive(true);
        _settingsMenuButtonBack.Select();
    }

    private void OnSettingsMenuButtonBackPressed()
    {
        _settingMenuWindow.SetActive(false);
        _mainMenuWindow.SetActive(true);
        _mainMenuButtonSettings.Select();
    }

    private void OnMainMenuButtonQuitGamePressed()
    {
        Application.Quit();
    }
}
