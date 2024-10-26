using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : Singleton<MainMenuManager>
{
    [Header("Title Screen")]
    [SerializeField] private GameObject _titleScreenWindow;
    [SerializeField] private Button _titleScreenButtonStart;
    [Header("Main Menu")]
    [SerializeField] private GameObject _mainMenuWindow;
    [SerializeField] private Button _mainMenuButtonNewGame;
    [SerializeField] private Button _mainMenuButtonSettings;
    [SerializeField] private Button _mainMenuButtonQuitGame;
    [Header("Main Menu")]
    [SerializeField] private GameObject _settingMenuWindow;
    [SerializeField] private Button _settingsMenuButtonBack;

    protected override void Awake()
    {
        base.Awake();
        _titleScreenButtonStart.Select();
    }

    private void OnEnable()
    {
        // title screen
        _titleScreenButtonStart.onClick.AddListener(OnTitleScreenButtonStartPressed);
        // main menu
        _mainMenuButtonNewGame.onClick.AddListener(OnMainMenuButtonNewGamePressed);
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
        _mainMenuButtonNewGame.onClick.RemoveListener(OnMainMenuButtonNewGamePressed);
        _mainMenuButtonSettings.onClick.RemoveListener(OnMainMenuButtonSettingsPressed);
        _mainMenuButtonQuitGame.onClick.RemoveListener(OnMainMenuButtonQuitGamePressed);
        // settings
        _settingsMenuButtonBack.onClick.RemoveListener(OnSettingsMenuButtonBackPressed);
    }

    private void OnTitleScreenButtonStartPressed()
    {
        _titleScreenWindow.SetActive(false);
        _mainMenuWindow.SetActive(true);
        _mainMenuButtonNewGame.Select();
        // код для инициализации
    }

    private void OnMainMenuButtonNewGamePressed()
    {
        // create new game
        SceneManager.LoadScene(1, LoadSceneMode.Single);// временно
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
