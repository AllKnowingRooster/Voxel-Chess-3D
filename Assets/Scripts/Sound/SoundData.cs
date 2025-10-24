using NUnit.Framework;
using UnityEngine;


[System.Serializable]
public class SoundData
{
    public AudioClip clip;
    public float cooldown;
    [HideInInspector] public float lastUsedTime;
    public float volume;

    public SoundData(AudioClip clip, float cooldown, float volume)
    {
        this.clip = clip;
        this.cooldown = cooldown;
        this.volume = volume;
        this.lastUsedTime = 0.0f;
    }

    public void UpdateLastUsedTime(float time)
    {
        lastUsedTime = time;
    }

}
