using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsPage : MonoBehaviour
{
    [SerializeField] private Slider masterVolumeSlider;
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider sfxVolumeSlider;
    [SerializeField] private TextMeshProUGUI masterVolumeText;
    [SerializeField] private TextMeshProUGUI musicVolumeText;
    [SerializeField] private TextMeshProUGUI sfxVolumeText;

    void Awake()
    {
        masterVolumeSlider.onValueChanged.RemoveAllListeners();
        musicVolumeSlider.onValueChanged.RemoveAllListeners();
        sfxVolumeSlider.onValueChanged.RemoveAllListeners();
        masterVolumeSlider.onValueChanged.AddListener((float val) => { AudioManager.instance.updateAudioMixer("Master Volume", masterVolumeText, val); });
        musicVolumeSlider.onValueChanged.AddListener((float val) => { AudioManager.instance.updateAudioMixer("Music Volume", musicVolumeText, val); });
        sfxVolumeSlider.onValueChanged.AddListener((float val) => { AudioManager.instance.updateAudioMixer("SFX Volume", sfxVolumeText, val); });
        float curval1;
        AudioManager.instance.audioMixer.GetFloat("Master Volume", out curval1);
        masterVolumeSlider.value = (curval1 - (-80)) / (0 - (-80));
        float curval2;
        AudioManager.instance.audioMixer.GetFloat("Music Volume", out curval2);
        musicVolumeSlider.value = (curval2 - (-80)) / (0 - (-80));
        float curval3;
        AudioManager.instance.audioMixer.GetFloat("SFX Volume", out curval3);
        sfxVolumeSlider.value = (curval3 - (-80)) / (0 - (-80));
    }
}
