using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuFunctions : MonoBehaviour
{
    private GameObject menuObj;
    private GameObject settingsObj;

    private Button playButton;
    private Button settingsButton;
    private Button tutorialButton;

    public string playButtonLevelName;
    public string tutorialButtonLevelName;


    public void Awake()
    {
        menuObj = GameObject.Find("Menu");
        settingsObj = GameObject.Find("SettingsPanel");



        Button[] childButtons = menuObj.GetComponentsInChildren<Button>();

        foreach (Button button in childButtons)
        {
            if (button.gameObject.name == "Play")
            {
                playButton = button;
            }

            if (button.gameObject.name == "Settings")
            {
                settingsButton = button;
            }
            if (button.gameObject.name == "Tutorial")
            {
                tutorialButton = button;
            }
        }
    }

    public void Start()
    {
        playButton.onClick.AddListener(() => LoadLevel(playButtonLevelName));
        settingsButton.onClick.AddListener(() => LoadSettings());

        settingsObj.SetActive(false);
    }


    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
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
