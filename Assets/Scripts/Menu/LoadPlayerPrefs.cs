using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using Unity.VisualScripting;

public class LoadPlayerPrefs : MonoBehaviour
{

    private GameObject settings;
    private GameObject title;


    private float musicVolume;
    private float sfxVolume;
    private Slider musicSlider;
    private Slider sfxSlider;

    public void Awake()
    {
        title = GameObject.Find("Title");
        settings = GameObject.Find("SettingsPanel");

        musicVolume = PlayerPrefs.GetFloat("musicVolume", 80f);
        sfxVolume = PlayerPrefs.GetFloat("sfxVolume", 80f);

        Slider[] sliders = settings.GetComponentsInChildren<Slider>();

        foreach (Slider slider in sliders)
        {
            if (slider.name == "MusicSlider")
            {
                musicSlider = slider;
            }
            if (slider.name == "SFXSlider")
            {
                sfxSlider = slider;
            }
        }


        musicSlider.value = musicVolume;
        sfxSlider.value = sfxVolume;
    }

    //Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Start()
    {

        musicSlider.onValueChanged.AddListener(delegate { UpdateFloatInPlayerCache("musicVolume", musicSlider.value); });
        sfxSlider.onValueChanged.AddListener(delegate { UpdateFloatInPlayerCache("sfxVolume", sfxSlider.value); });

        //settings.SetActive(false);
        //title.SetActive(false);
    }

    public void UpdateFloatInPlayerCache(String cacheName, float value)
    {
        PlayerPrefs.SetFloat(cacheName, value);
    }

    public void UpdateIntInPlayerCache (String cacheName, int value)
    {
        PlayerPrefs.SetFloat(cacheName, value);
    }

}

