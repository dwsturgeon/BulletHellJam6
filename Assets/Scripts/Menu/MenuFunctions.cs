using System;
using Unity.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuFunctions : MonoBehaviour
{
    private GameObject menu;
    private GameObject settingsObj;
    public string levelName;


    public void Awake()
    {
        menu = GameObject.Find("Menu");
        settingsObj = GameObject.Find("SettingsPanel");
    }

    public void loadLevel()
    {
        SceneManager.LoadScene(levelName);
    }

    public void ActivateSettings()
    {
        menu.SetActive(false);
        settingsObj.SetActive(true);
    }

}
