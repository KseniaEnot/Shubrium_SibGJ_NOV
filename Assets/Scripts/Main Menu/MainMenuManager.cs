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
    [SerializeField] private Button _mainMenuButtonStartGame;
    [SerializeField] private Button _mainMenuButtonSettings;
    [SerializeField] private Button _mainMenuButtonQuitGame;
    [Header("Settings Menu")]
    [SerializeField] private GameObject _settingMenuWindow;
    [SerializeField] private Scrollbar _masterScrollbar;
    [SerializeField] private Scrollbar _musicScrollbar;
    [SerializeField] private Scrollbar _SFXScrollbar;
    [SerializeField] private Button _settingsMenuButtonBack;

    protected override void Awake()
    {
        base.Awake();
        _titleScreenButtonStart.Select();
        _masterScrollbar.value = AudioPreferences.LoadVolume(VolumeType.Master);
        _musicScrollbar.value = AudioPreferences.LoadVolume(VolumeType.Ambient);
        _SFXScrollbar.value = AudioPreferences.LoadVolume(VolumeType.SFX);
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
        //_masterScrollbar.onValueChanged.AddListener(OnMasterVolumeChanged);
        //_musicScrollbar.onValueChanged.AddListener(OnMusicVolumeChanged);
        //_SFXScrollbar.onValueChanged.AddListener(OnSFXVolumeChanged);
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
        //_masterScrollbar.onValueChanged.RemoveListener(OnMasterVolumeChanged);
        //_musicScrollbar.onValueChanged.RemoveListener(OnMusicVolumeChanged);
        //_SFXScrollbar.onValueChanged.RemoveListener(OnSFXVolumeChanged);
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

    //private void OnMasterVolumeChanged(float value) => WorldSoundManager.StaticInstance.SetVolume(WorldSoundManager.StaticInstance.Master, VolumeType.Master, value);
    //private void OnMusicVolumeChanged(float value) => WorldSoundManager.StaticInstance.SetVolume(WorldSoundManager.StaticInstance.Music, VolumeType.Ambient, value);
    //private void OnSFXVolumeChanged(float value) => WorldSoundManager.StaticInstance.SetVolume(WorldSoundManager.StaticInstance.SFX, VolumeType.SFX, value);

    private void OnMainMenuButtonQuitGamePressed()
    {
        Application.Quit();
    }
}
