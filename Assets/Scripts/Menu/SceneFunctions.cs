using System;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class SceneFunctions : MonoBehaviour
{

    private GameObject settingsObj;


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
                Time.timeScale = 1f;
            }
            else
            {
                settingsObj.SetActive(true);
                Time.timeScale = 0f;
            }
        }
    }

}
