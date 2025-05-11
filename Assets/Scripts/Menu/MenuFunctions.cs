using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class MenuFunctions : MonoBehaviour
{
    private GameObject menuObj;
    private GameObject settingsObj;



    [SerializeField]  Image Title;
    [SerializeField]  Button playButton;
    [SerializeField]  Button settingsButton;
    [SerializeField]  Button tutorialButton;


    public string playButtonLevelName;
    public string tutorialButtonLevelName;


    public void Awake()
    {
        menuObj = GameObject.Find("Menu");
        settingsObj = GameObject.Find("SettingsPanel");
    }

    public void Start()
    {
        playButton.onClick.AddListener(() => LoadLevel(playButtonLevelName));
        settingsButton.onClick.AddListener(() => LoadSettings());
        tutorialButton.onClick.AddListener(() => LoadLevel(tutorialButtonLevelName));

        settingsObj.SetActive(false);
    }


    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (settingsObj.activeSelf)
            {
                menuObj.SetActive(true);
                settingsObj.SetActive(false);
            }
        }
    }

    public void LoadSettings()
    {
        menuObj.SetActive(false);
        settingsObj.SetActive(true);

    }
    public void LoadLevel(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }

    public void ActivateSettings()
    {
        menuObj.SetActive(false);
        settingsObj.SetActive(true);
    }
}
