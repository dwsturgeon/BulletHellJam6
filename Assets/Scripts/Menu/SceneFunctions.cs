using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneFunctions : MonoBehaviour
{
    private GameObject settingsObj;
    private GameObject endScreenObj;

    [SerializeField] Button Menu;
    [SerializeField] Button Restart;

    [SerializeField] string currentSceneName = "MainTesting";
    [SerializeField] string titleSceneName = "Title";

    public bool settingsUseable = true;
    void Awake()
    {
        settingsObj = GameObject.Find("SettingsPanel");
        settingsObj.SetActive(false);

        endScreenObj = GameObject.Find("EndrunScreen");
        endScreenObj.SetActive(false);

        currentSceneName = SceneManager.GetActiveScene().name;

        Time.timeScale = 1.0f;
    }

    private void Start()
    {
        Restart.onClick.AddListener(delegate {
            LoadLevel(currentSceneName);
        });

        Menu.onClick.AddListener(delegate {
            LoadLevel(titleSceneName);
        });


    }



    void Update()
    {
        if (settingsUseable)
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                if (settingsObj.activeSelf)
                {
                    settingsObj.SetActive(false);
                    Time.timeScale = 1f;
                    PlayerController.instance.isPaused = false;
                    PlayerController.instance.gameObject.GetComponent<ShakeOnCollision>().isPaused = false;
                }
                else
                {
                    settingsObj.SetActive(true);
                    Time.timeScale = 0f;
                    PlayerController.instance.isPaused = true;
                    PlayerController.instance.gameObject.GetComponent<ShakeOnCollision>().isPaused = true;
                }
            }
        }
    }

    void LoadLevel(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }

}
