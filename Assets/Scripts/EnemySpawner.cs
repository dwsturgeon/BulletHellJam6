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



    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        LoadNextWave();
    }

    // Update is called once per frame
    void Update()
    {
        currentWave = GetCurrentWave();
        if (currentWave != null)
        {
            Debug.Log(currentWave.name);
        }

        currentEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        if(currentEnemies.Length == 0)
        {
            Destroy(instantiatedObject);
            LoadNextWave();
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
