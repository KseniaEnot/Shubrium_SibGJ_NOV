using UnityEngine;

[CreateAssetMenu(fileName = "FMODBusData", menuName = "Audio/FMOD Bus Data")]
public class FMODBusData : ScriptableObject
{
	[Header("Bus paths")]
	[SerializeField] private string _masterBusPath;
	[SerializeField] private string _musicBusPath;
	[SerializeField] private string _SFXBusPath;

	private FMOD.Studio.Bus _master;
	private FMOD.Studio.Bus _music;
	private FMOD.Studio.Bus _SFX;

	public void InitializeBuses()
	{
		_master = FMODUnity.RuntimeManager.GetBus(_masterBusPath);
		_music = FMODUnity.RuntimeManager.GetBus(_musicBusPath);
		_SFX = FMODUnity.RuntimeManager.GetBus(_SFXBusPath);

		SetMasterVolume(AudioPreferences.LoadVolume(VolumeType.Master));
		SetMusicVolume(AudioPreferences.LoadVolume(VolumeType.Music));
		SetSFXVolume(AudioPreferences.LoadVolume(VolumeType.SFX));
	}

	public void SetMasterVolume(float volume)
	{
		_master.setVolume(volume);
		AudioPreferences.SetVolume(VolumeType.Master, volume);
	}

	public void SetMusicVolume(float volume)
	{
		_music.setVolume(volume);
		AudioPreferences.SetVolume(VolumeType.Music, volume);
	}

	public void SetSFXVolume(float volume)
	{
		_SFX.setVolume(volume);
		AudioPreferences.SetVolume(VolumeType.SFX, volume);
	}
}
