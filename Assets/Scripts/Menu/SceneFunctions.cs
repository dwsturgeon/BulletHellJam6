using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneFunctions : MonoBehaviour
{

    private GameObject settingsObj;


    public string playButtonLevelName;
    public string tutorialButtonLevelName;


    void Awake()
    {
        settingsObj = GameObject.Find("SettingsPanel");
    }

    void Start()
    {
        settingsObj.SetActive(false);
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (settingsObj.activeSelf)
            {
                settingsObj.SetActive(false);
            }
            else
            {
                settingsObj.SetActive(true);
            }
        }
    }

    void LoadLevel(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }

}
