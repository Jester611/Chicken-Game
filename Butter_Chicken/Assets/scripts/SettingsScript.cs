using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;
public class SettingsScript : MonoBehaviour
{
    [SerializeField] Slider sfxVolumeSlider;
    [SerializeField] Slider musicVolumeSlider;

    [SerializeField] AudioMixer audioMixer;
    
    [SerializeField] TextMeshProUGUI sfxVolumeText;
    [SerializeField] TextMeshProUGUI musicVolumeText;


    private void Awake() {
        SetFromPrefs();
        //SetSliders();
    }

    public void SetSFXVolume(float value){
        PlayerPrefs.SetFloat("sfxvolume", value);
        sfxVolumeText.text = ((int)(value*100)).ToString();
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(value)*20);
    }

    public void SetMusicVolume(float value){
        PlayerPrefs.SetFloat("musicvolume", value);
        musicVolumeText.text = ((int)(value*100)).ToString();
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(value)*20);

    }

    private void SetFromPrefs(){
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(PlayerPrefs.GetFloat("musicvolume")*20));
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(PlayerPrefs.GetFloat("sfxvolume"))*20);

    }

    private void SetSliders(){
        if(PlayerPrefs.HasKey("sfxvolume")){
            sfxVolumeSlider.value = PlayerPrefs.GetFloat("sfxvolume");
        }
        else{
            PlayerPrefs.SetFloat("sfxvolume", 0);
            sfxVolumeSlider.value = 0.8f;
        }
        if(PlayerPrefs.HasKey("musicvolume")){
            musicVolumeSlider.value = PlayerPrefs.GetFloat("musicvolume");
        }
        else{
            PlayerPrefs.SetFloat("musicvolume", 0);
            musicVolumeSlider.value = 0.8f;
        }
    }
}
