using UnityEngine;
using FMODUnity;

[CreateAssetMenu(fileName = "NewSoundEvent", menuName = "Audio/FMOD Sound Event")]
public class FMODEvent : ScriptableObject
{
	[SerializeField] private EventReference _eventReference;
	public EventReference EventReference => _eventReference;

	private FMOD.Studio.EventInstance _eventInstance;

	public void Play()
	{
		_eventInstance = RuntimeManager.CreateInstance(_eventReference);
		_eventInstance.start();
	}

	public void Stop(FMOD.Studio.STOP_MODE stopMode = FMOD.Studio.STOP_MODE.IMMEDIATE)
	{
		if (_eventInstance.isValid())
		{
			_eventInstance.stop(stopMode);
			_eventInstance.release();
		}
	}

	public void ReleaseEvent()
	{
		if (_eventInstance.isValid())
		{
			_eventInstance.release();
		}
	}
}
