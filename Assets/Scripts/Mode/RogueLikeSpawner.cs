using Unity.VisualScripting;
using UnityEngine;

public class RogueLikeSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] lowLevelEnemies;
    [SerializeField] private GameObject[] mediumLevelEnemies;
    [SerializeField] private GameObject[] highLevelEnemies;
    [SerializeField] private GameObject[] bossLevelEnemies;

    [Header("Spawn Settings")]
    [SerializeField] private float minX;
    [SerializeField] private float maxX;
    [SerializeField] private float minY;
    [SerializeField] private float maxY;

    [Header(("Wave Settings"))]
    [SerializeField] float timeBetweenWaves = 2f;
    private float countdown = 0f;
    private int currentWave = 0;
    private bool waveInProgress = false;

    private void Start()
    {
        Debug.Log(highLevelEnemies.Length);
    }

    private void FixedUpdate()
    {
        if(!waveInProgress && AllEnemiesDead())
        {
            countdown -= Time.fixedDeltaTime;

            if(countdown <= 0f)
            {
                StartWave();
                countdown = timeBetweenWaves;
            }
        }
    }



    void StartWave()
    {
        currentWave++;
        Debug.Log("Starting Wave " +  currentWave);

        waveInProgress = true;

        if (currentWave % 5 == 0)
        {
            SpawnBoss();
        }
        else
        {
            SpawnEnemies();
        }

        waveInProgress = false;
    }

    void SpawnEnemies()
    {
        GameObject[] enemyArray = GetEnemyArrayForWave();

        int enemiesToSpawn = Mathf.Min(3 + currentWave / 5, 5);

        for(int i = 0; i < enemiesToSpawn; i++)
        {
            GameObject enemyPrefab = enemyArray[Random.Range(0, enemyArray.Length)];
            Vector2 spawnPoint = GetRandomPoint();
            Instantiate(enemyPrefab, spawnPoint, Quaternion.identity);
        }
    }

    void SpawnBoss()
    {
        GameObject boss = bossLevelEnemies[Random.Range(0, bossLevelEnemies.Length)];
        Vector2 spawnPoint = GetRandomPoint();
        Instantiate(boss, spawnPoint, Quaternion.identity);
    }

    GameObject[] GetEnemyArrayForWave()
    {
        if (currentWave < 2)
        {
            return lowLevelEnemies;
        }
        else if (currentWave < 8)
        {
            return mediumLevelEnemies;
        }
        else return highLevelEnemies;
    }

    bool AllEnemiesDead()
    {
        return GameObject.FindGameObjectsWithTag("Enemy").Length == 0;
    }

    private Vector3 GetRandomPoint()
    {
        float randomX = Random.Range(minX, maxX);
        float randomY = Random.Range(minY, maxY);
        return new Vector3(randomX, randomY, 0);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(new Vector3((minX + maxX) / 2f, (minY + maxY) / 2f, 0f), new Vector3(maxX - minX, maxY - minY, 0));
    }

}



