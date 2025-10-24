using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour,IObserver
{

    public static AudioManager instance { get; private set; }
    private Dictionary<UserAction, SoundData> soundMap;
    [SerializeField] private List<SoundData> listSoundEffect;
    [SerializeField] private List<SoundData> listBGM;
    [SerializeField] private AudioSource audioSourceSFX;
    [SerializeField] private AudioSource audioSourceBGM;
    [SerializeField] private Slider masterVolumeSlider;
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider sfxVolumeSlider;
    [SerializeField] private TextMeshProUGUI masterVolumeText;
    [SerializeField] private TextMeshProUGUI musicVolumeText;
    [SerializeField] private TextMeshProUGUI sfxVolumeText;
    [SerializeField] private AudioMixer audioMixer;
    public void OnNotify(UserAction action)
    {
        SoundData sfx= soundMap[action];
        if (sfx == null || (Time.time-sfx.cooldown<sfx.lastUsedTime))
        {
            return;
        }

        audioSourceSFX.PlayOneShot(sfx.clip, sfx.volume);

        sfx.UpdateLastUsedTime(Time.time);
    }

    private void OnEnable()
    {
        CanvasManager.instance.AddObserver(this);
    }

    private void OnDisable()
    {
        CanvasManager.instance.RemoveObserver(this);
    }
    private void Awake()
    {
        if (instance!=null)
        {
            return;
        }

        instance= this;
        soundMap = new Dictionary<UserAction, SoundData>();
        UserAction[] listEnumValues = (UserAction[])Enum.GetValues(typeof(UserAction));
        for (int i=0;i<listSoundEffect.Count;i++)
        {
            soundMap[listEnumValues[i]] = listSoundEffect[i];
        }
        DontDestroyOnLoad(instance);
        audioSourceBGM.clip = listBGM[0].clip;
        audioSourceBGM.Play();
        masterVolumeSlider.onValueChanged.RemoveAllListeners();
        musicVolumeSlider.onValueChanged.RemoveAllListeners();
        sfxVolumeSlider.onValueChanged.RemoveAllListeners();
        masterVolumeSlider.onValueChanged.AddListener((float val) => {updateAudioMixer("Master Volume",masterVolumeText, val); });
        musicVolumeSlider.onValueChanged.AddListener((float val) => { updateAudioMixer("Music Volume",musicVolumeText, val); });
        sfxVolumeSlider.onValueChanged.AddListener((float val) => { updateAudioMixer("SFX Volume",sfxVolumeText, val); });
        masterVolumeSlider.value = 0.5f;
        musicVolumeSlider.value = 0.5f;
        sfxVolumeSlider.value = 0.5f;
    }


    void updateAudioMixer(string groupParam ,TextMeshProUGUI text,float value)
    {
        float toDecibel = Mathf.Clamp(Mathf.Log10(value) * 20, -80.0f, 0);
        audioMixer.SetFloat(groupParam,toDecibel);
        text.text = Mathf.Floor(value / 1 * 100).ToString()+"%";
    }


}
