using FMODUnity;
using UnityEngine;
using UnityEngine.UI;

public class AudioSettings : MonoBehaviour
{
	[SerializeField] private Scrollbar _masterScrollbar;
	[SerializeField] private Scrollbar _musicScrollbar;
	[SerializeField] private Scrollbar _sfxScrollbar;

	private FMOD.Studio.Bus Master;
	private FMOD.Studio.Bus Music;
	private FMOD.Studio.Bus SFX;


	private void Awake()
	{
		// Master = RuntimeManager.GetBus("bus:/Master");
		// Music = RuntimeManager.GetBus("bus:/Master/Music");
		// SFX = RuntimeManager.GetBus("bus:/Master/SFX");

		_masterScrollbar.value = AudioPreferences.LoadVolume(VolumeType.Master);
		_musicScrollbar.value = AudioPreferences.LoadVolume(VolumeType.Music);
		_sfxScrollbar.value = AudioPreferences.LoadVolume(VolumeType.SFX);
	}

	private void OnEnable()
	{
		_masterScrollbar.onValueChanged.AddListener(OnMasterVolumeChanged);
		_musicScrollbar.onValueChanged.AddListener(OnMusicVolumeChanged);
		_sfxScrollbar.onValueChanged.AddListener(OnSFXVolumeChanged);
	}

	private void OnDisable()
	{
		_masterScrollbar.onValueChanged.RemoveListener(OnMasterVolumeChanged);
		_musicScrollbar.onValueChanged.RemoveListener(OnMusicVolumeChanged);
		_sfxScrollbar.onValueChanged.RemoveListener(OnSFXVolumeChanged);
	}

	private void OnMasterVolumeChanged(float value) => SetVolume(Master, VolumeType.Master, value);
	private void OnMusicVolumeChanged(float value) => SetVolume(Music, VolumeType.Music, value);
	private void OnSFXVolumeChanged(float value) => SetVolume(SFX, VolumeType.SFX, value);

	private void SetVolume(FMOD.Studio.Bus audioBus, VolumeType volumeType, float volume)
	{
		Debug.Log($"new value of {volumeType} is {volume}");
		audioBus.setVolume(volume);
		AudioPreferences.SaveVolume(volumeType, volume);
	}
}
