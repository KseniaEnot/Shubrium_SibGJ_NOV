using FMODUnity;
using UnityEngine;
using UnityEngine.UI;

public class AudioSettings : MonoBehaviour
{
    private FMOD.Studio.Bus _master;
    private FMOD.Studio.Bus _music;
    private FMOD.Studio.Bus _SFX;

    [SerializeField] private Scrollbar _masterScrollbar;
    [SerializeField] private Scrollbar _musicScrollbar;
    [SerializeField] private Scrollbar _SFXScrollbar;

    private void Awake()
    {
        _masterScrollbar.value = AudioPreferences.LoadVolume(VolumeType.Master);
        _musicScrollbar.value = AudioPreferences.LoadVolume(VolumeType.Ambient);
        _SFXScrollbar.value = AudioPreferences.LoadVolume(VolumeType.SFX);
        _masterScrollbar.onValueChanged.AddListener(OnMasterVolumeChanged);
        _musicScrollbar.onValueChanged.AddListener(OnMusicVolumeChanged);
        _SFXScrollbar.onValueChanged.AddListener(OnSFXVolumeChanged);
    }

    private void OnMasterVolumeChanged(float value) => SetVolume(_master, VolumeType.Master, value);
    private void OnMusicVolumeChanged(float value) => SetVolume(_music, VolumeType.Ambient, value);
    private void OnSFXVolumeChanged(float value) => SetVolume(_SFX, VolumeType.SFX, value);

    public void SetVolume(FMOD.Studio.Bus audiobus, VolumeType volumetype, float volume)
    {
        Debug.Log($"new value of {volumetype} is {volume}");
        audiobus.setVolume(volume);
        AudioPreferences.SaveVolume(volumetype, volume);
    }
}
