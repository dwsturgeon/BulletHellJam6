using System;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField] GameObject[] wavePrefabs;


    private GameObject currentWave = null;
    private GameObject instantiatedObject;
    private GameObject[] currentEnemies;
    
    public static EnemySpawner instance;

    private float timeToWait = 2f;
    private float timeElapsed = 0;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        //init first wave
        LoadNextWave();
    }

    // Update is called once per frame
    void Update()
    {
        //every 2 seconds check and init/destroy waves
        timeElapsed += Time.deltaTime;
        if (timeElapsed > timeToWait)
        {
            timeElapsed = 0;

            //grab current wave
            currentWave = GetCurrentWave();
            if (currentWave != null)
            {
                Debug.Log(currentWave.name);
            }
            //grab current enemies
            currentEnemies = GameObject.FindGameObjectsWithTag("Enemy");

            //destroy wave gameobject and load next
            if (currentEnemies.Length == 0)
            {
                Destroy(instantiatedObject);
                LoadNextWave();
            }
        }
    }


    private void LoadNextWave()
    {
        if (wavePrefabs.Length > 0)
        {
            instantiatedObject = Instantiate(wavePrefabs[0]);
            instantiatedObject.transform.SetParent(gameObject.transform);
        }
    }
    private GameObject GetCurrentWave()
    {
        GameObject wave = null;

        GameObject[] waves = GameObject.FindGameObjectsWithTag("Wave");


        //get first wave should only have 1 at a time.
        for (int i = 0; i < waves.Length; i++) 
        {
            if (waves[i].tag == "Wave")
            {
                wave = waves[i];
                break;
            }
        }

        return wave;
    }
}
