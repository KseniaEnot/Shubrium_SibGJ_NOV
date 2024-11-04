using FMOD.Studio;
using FMODUnity;
using UnityEngine;

public class CharacterAudioManager : MonoBehaviour
{
    [SerializeField] private StudioEventEmitter _stepEmitter;
    [SerializeField] private StudioEventEmitter _startEmitter;
    [SerializeField] private StudioEventEmitter _acceptEmitter;
    [SerializeField] private StudioEventEmitter _denyEmitter;

    private EventInstance _eventInstance;

    public void PlayStep()
    {
        _stepEmitter.Play();
    }

    public void PlayStartConservationQuestSound()
    {
        _startEmitter.Play();
    }

    public void PlayAcceptQuestSound()
    {
        _acceptEmitter.Play();
    }

    public void PlayDenyQuestSound()
    {
        _denyEmitter.Play();
    }
}
