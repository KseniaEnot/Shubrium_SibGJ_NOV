using UnityEngine;
using FMODUnity;

[CreateAssetMenu(fileName = "NewSoundEvent", menuName = "Audio/FMOD Sound Event")]
public class FMODEvent : ScriptableObject
{
	[SerializeField] private EventReference _eventReference;
	public EventReference EventReference => _eventReference;
}
