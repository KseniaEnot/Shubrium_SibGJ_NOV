using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class AudioManager : Singleton<AudioManager>
{
	[SerializeField] private FMODBusData _FMODBusData;

	private void Start()
	{
		InitializeAudioVolume();
	}
	
	private void InitializeAudioVolume()
	{
		_FMODBusData.InitializeBuses();
	}
	
	public void PlayOneShot(EventReference sound, Vector3 worldPos)
	{
		RuntimeManager.PlayOneShot(sound, worldPos);
	}
	
	public EventInstance CreateEventInstance(EventReference eventReference)
	{
		EventInstance eventInstance = RuntimeManager.CreateInstance(eventReference);
		return eventInstance;
	}
}
