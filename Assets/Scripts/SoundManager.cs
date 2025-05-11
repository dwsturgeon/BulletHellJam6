using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{

    private static SoundManager instance;
    [SerializeField] AudioMixer audioMixer;

    [SerializeField] Slider musicSlider;
    [SerializeField] Slider sfxSlider;

    public float masterVolume = 1.0f; // Initial value

    private float musicVolume;
    private float sfxVolume;

    const string MIXER_MUSIC = "MusicVolume";
    const string MIXER_SFX = "SFXVolume";




    void Awake()
    {
        instance = this;

        musicVolume = PlayerPrefs.GetFloat("musicVolume", .80f);
        sfxVolume = PlayerPrefs.GetFloat("sfxVolume", .80f);

        musicSlider.value = musicVolume;
        sfxSlider.value = sfxVolume;

        SetMusicVolume();
        SetSFXVolume();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetMusicVolume()
    {
        PlayerPrefs.SetFloat("musicVolume", musicSlider.value);
        audioMixer.SetFloat(MIXER_MUSIC, (Mathf.Log10(musicSlider.value) * 20));
    }

    public void SetSFXVolume()
    {
        PlayerPrefs.SetFloat("sfxVolume", sfxSlider.value);
        audioMixer.SetFloat(MIXER_SFX, (Mathf.Log10(sfxSlider.value) * 20));
        //Debug.Log((Mathf.Log10(sfxSlider.value) * 20));
    }

    
}
