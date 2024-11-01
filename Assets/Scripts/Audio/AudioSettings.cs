using UnityEngine;
using UnityEngine.UI;

public class AudioSettings : MonoBehaviour
{
	[Header("Scroll Bars")]
	[SerializeField] private Scrollbar _masterScrollbar;
	[SerializeField] private Scrollbar _musicScrollbar;
	[SerializeField] private Scrollbar _SFXScrollbar;
	
	[Header("FMOD Bus data asset")]
	[SerializeField] private FMODBusData _busData;

	private void OnEnable()
	{
		InitializeBars();
		
		_masterScrollbar.onValueChanged.AddListener(OnMasterVolumeChanged);
		_musicScrollbar.onValueChanged.AddListener(OnMusicVolumeChanged);
		_SFXScrollbar.onValueChanged.AddListener(OnSFXVolumeChanged);
	}
	
	private void OnDisable()
	{
		_masterScrollbar.onValueChanged.RemoveListener(OnMasterVolumeChanged);
		_musicScrollbar.onValueChanged.RemoveListener(OnMusicVolumeChanged);
		_SFXScrollbar.onValueChanged.RemoveListener(OnSFXVolumeChanged);
	}

	private void InitializeBars()
	{
		_masterScrollbar.value = AudioPreferences.LoadVolume(VolumeType.Master);
		_musicScrollbar.value = AudioPreferences.LoadVolume(VolumeType.Music);
		_SFXScrollbar.value = AudioPreferences.LoadVolume(VolumeType.SFX);
	}

	public void OnMasterVolumeChanged(float value) => _busData.SetMasterVolume(value);
	public void OnMusicVolumeChanged(float value) => _busData.SetMusicVolume(value);
	public void OnSFXVolumeChanged(float value) => _busData.SetSFXVolume(value);
}
