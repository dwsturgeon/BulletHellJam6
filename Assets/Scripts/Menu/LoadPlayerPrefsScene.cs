using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using Unity.VisualScripting;

public class LoadPlayerPrefsScene : MonoBehaviour
{

    private bool menuActive;

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

        title.SetActive(false);
        settings.SetActive(false);

    }

    //Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Start()
    {

        musicSlider.onValueChanged.AddListener(delegate { UpdateFloatInPlayerCache("musicVolume", musicSlider.value); });
        sfxSlider.onValueChanged.AddListener(delegate { UpdateFloatInPlayerCache("sfxVolume", sfxSlider.value); });
    }

    public void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (menuActive)
            {
                title.SetActive(true);
                settings.SetActive(true);
                Time.timeScale = 0;
                menuActive = true;
            }
            else
            {
                title.SetActive(false);
                settings.SetActive(false);
                Time.timeScale = 1;
                menuActive = false;
            }
        }
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

