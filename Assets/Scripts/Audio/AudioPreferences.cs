using UnityEngine;

public static class AudioPreferences
{
	public static void SaveVolume(VolumeType volumeType, float volume)
	{
		PlayerPrefs.SetFloat(volumeType.ToString(), volume);
	}

	public static float LoadVolume(VolumeType volumeType)
	{
		return PlayerPrefs.GetFloat(volumeType.ToString(), 0.75f);
	}
}
