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

	public string MasterBusPath => _masterBusPath;
	public string MusicBusPath => _musicBusPath;
	public string SFXBusPath => _SFXBusPath;

	public void InitializeBuses()
	{
		_master = FMODUnity.RuntimeManager.GetBus(MasterBusPath);
		_music = FMODUnity.RuntimeManager.GetBus(MusicBusPath);
		_SFX = FMODUnity.RuntimeManager.GetBus(SFXBusPath);

		SetMasterVolume(AudioPreferences.LoadVolume(VolumeType.Master));
		SetMusicVolume(AudioPreferences.LoadVolume(VolumeType.Music));
		SetSFXVolume(AudioPreferences.LoadVolume(VolumeType.SFX));
	}

	public void SetMasterVolume(float volume)
	{
		_master.setVolume(volume);
		AudioPreferences.SaveVolume(VolumeType.Master, volume);
	}

	public void SetMusicVolume(float volume)
	{
		_music.setVolume(volume);
		AudioPreferences.SaveVolume(VolumeType.Music, volume);
	}

	public void SetSFXVolume(float volume)
	{
		_SFX.setVolume(volume);
		AudioPreferences.SaveVolume(VolumeType.SFX, volume);
	}
}
