using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour, IObserver
{

    public static AudioManager instance { get; private set; }
    private Dictionary<UserAction, SoundData> soundMap;
    [SerializeField] private List<SoundData> listSoundEffect;
    [SerializeField] private List<SoundData> listBGM;
    [SerializeField] private AudioSource audioSourceSFX;
    [SerializeField] private AudioSource audioSourceBGM;
    public AudioMixer audioMixer;
    public void OnNotify(UserAction action)
    {
        SoundData sfx = soundMap[action];
        if (sfx == null || (Time.time - sfx.cooldown < sfx.lastUsedTime))
        {
            return;
        }
        audioSourceSFX.PlayOneShot(sfx.clip, sfx.volume);
        sfx.UpdateLastUsedTime(Time.time);
    }

    private void OnEnable()
    {
        GameManager.instance.AddObserver(this);
    }

    private void OnDisable()
    {
        GameManager.instance.RemoveObserver(this);
    }



    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        soundMap = new Dictionary<UserAction, SoundData>();
        UserAction[] listEnumValues = (UserAction[])Enum.GetValues(typeof(UserAction));
        for (int i = 0; i < listSoundEffect.Count; i++)
        {
            soundMap[listEnumValues[i]] = listSoundEffect[i];
        }
        DontDestroyOnLoad(instance);
        audioSourceBGM.clip = listBGM[0].clip;
        audioSourceBGM.Play();
    }


    public void updateAudioMixer(string groupParam, TextMeshProUGUI text, float value)
    {
        float toDecibel = Mathf.Clamp(Mathf.Log10(value) * 20, -80.0f, 0);
        audioMixer.SetFloat(groupParam, toDecibel);
        text.text = Mathf.Floor(value * 100.0f).ToString() + "%";
    }


}
