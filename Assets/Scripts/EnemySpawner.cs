using System;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
   
    [SerializeField] GameObject[] wavePrefabs;
    private int wavePrefabPos = 0;


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
        if (wavePrefabPos + 1 <= wavePrefabs.Length)
        {
            instantiatedObject = Instantiate(wavePrefabs[wavePrefabPos]);
            instantiatedObject.transform.SetParent(gameObject.transform);
            wavePrefabPos += 1;
        }
        
    }
    private GameObject GetCurrentWave()
    {
        GameObject wave = null;

        GameObject[] waves = GameObject.FindGameObjectsWithTag("Wave");


        //get first wave should only have 1 at a time.
        if (waves != null)
        {
            for (int i = 0; i < waves.Length; i++)
            {
                if (waves[i].tag == "Wave")
                {
                    wave = waves[i];
                    break;
                }
            }
        }

        return wave;
    }
}
