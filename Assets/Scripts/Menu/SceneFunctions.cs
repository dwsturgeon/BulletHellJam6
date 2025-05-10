using UnityEngine;

public class SceneFunctions : MonoBehaviour
{
    private GameObject settingsObj;
    private GameObject endScreenObj;


    void Awake()
    {
        settingsObj = GameObject.Find("SettingsPanel");
        settingsObj.SetActive(false);

        endScreenObj = GameObject.Find("EndrunScreen");
        endScreenObj.SetActive(false);

    }



    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (settingsObj.activeSelf)
            {
                settingsObj.SetActive(false);
                Time.timeScale = 1f;
                PlayerController.instance.isPaused = false;
            }
            else
            {
                settingsObj.SetActive(true);
                Time.timeScale = 0f;
                PlayerController.instance.isPaused = true;
            }
        }
    }

}
