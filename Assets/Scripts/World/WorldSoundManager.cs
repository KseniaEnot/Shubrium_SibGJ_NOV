using FMODUnity;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WorldSoundManager : Singleton<WorldSoundManager>
{
    private FMOD.Studio.Bus _master;
    private FMOD.Studio.Bus _music;
    private FMOD.Studio.Bus _SFX;

    private Coroutine _changeMusicCoroutine;

    [SerializeField] private AudioClip _mainMenuMusicClip;
    [SerializeField] private AudioClip _gameMusicClip;
    [SerializeField] private float _changeMusicVolumeSpeed = 0.5f;

    public FMOD.Studio.Bus Master => _master;
    public FMOD.Studio.Bus Music => _music;
    public FMOD.Studio.Bus SFX => _SFX;

    protected override void Awake()
    {
        base.Awake();
        //_master = RuntimeManager.GetBus("bus:/Master");
        //_music = RuntimeManager.GetBus("bus:/Master/Ambient");
        //_SFX = RuntimeManager.GetBus("bus:/Master/SFX");
        SceneManager.activeSceneChanged += OnSceneChanged;
    }

    private void OnSceneChanged(Scene oldScene, Scene newScene)
    {
        if (newScene.buildIndex == 0)
        {
            ChangeMusic(_mainMenuMusicClip);
        }
        else
        {
            ChangeMusic(_gameMusicClip);
        }
    }

    public void SetVolume(FMOD.Studio.Bus audioBus, VolumeType volumeType, float volume)
    {
        Debug.Log($"New value of {volumeType} is {volume}");
        audioBus.setVolume(volume);
        AudioPreferences.SaveVolume(volumeType, volume);
    }

    public void ChangeMusic(AudioClip clip)
    {
        if (_changeMusicCoroutine != null)
        {
            StopCoroutine(_changeMusicCoroutine);
        }
        _changeMusicCoroutine = StartCoroutine(ChangeMusicTimer(clip));
    }

    private IEnumerator ChangeMusicTimer(AudioClip clip)
    {
        _music.getVolume(out float volume);
        while (volume > 0f)
        {
            volume -= _changeMusicVolumeSpeed * Time.deltaTime;
            _music.setVolume(volume);
            yield return null;
        }
        _music.setVolume(0f);
        // set music clip
        // music play
        while (volume < 1f)
        {
            volume += _changeMusicVolumeSpeed * Time.deltaTime;
            _music.setVolume(volume);
            yield return null;
        }
        _music.setVolume(1f);
        _changeMusicCoroutine = null;
    }
}
