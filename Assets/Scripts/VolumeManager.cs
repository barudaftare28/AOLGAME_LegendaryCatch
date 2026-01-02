using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
public class VolumeManager : MonoBehaviour
{
    [SerializeField] private AudioMixer mixer;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider SFXSlider;

    void Start()
    {
        if (PlayerPrefs.HasKey("musicVolume"))
        {
            loadVolume();
        }else
        {   
            setMusicVolume();
            setSFXVolume();
        }
          //set volume agar sesuai dengan mixer diawal 
    }
    public void setMusicVolume()
    {
        float volume = musicSlider.value;
        mixer.SetFloat("music",Mathf.Log10(volume)*20); 
        PlayerPrefs.SetFloat("musicVolume", volume);

    }
    public void setSFXVolume()
    {
        float volume = SFXSlider.value;
        mixer.SetFloat("SFX",Mathf.Log10(volume)*20); 
        PlayerPrefs.SetFloat("SFXVolume", volume);

    }
    private void loadVolume()
    {
        musicSlider.value = PlayerPrefs.GetFloat("musicVolume");
        SFXSlider.value = PlayerPrefs.GetFloat("SFXVolume");
        // setVolume();
        setSFXVolume();
        setMusicVolume();
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
}
