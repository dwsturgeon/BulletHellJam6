using TMPro;
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
    [SerializeField] private TextMeshProUGUI WaveCounterText;

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
        WaveCounterText.text = currentWave.ToString();
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

        int enemiesToSpawn = Mathf.Min(3 + currentWave / 5, 10);

        for(int i = 0; i < enemiesToSpawn; i++)
        {
            GameObject enemyPrefab = enemyArray[Random.Range(0, enemyArray.Length)];
            Vector2 spawnPoint = GetRandomPoint();
            GameObject target = Instantiate(enemyPrefab, spawnPoint, Quaternion.identity);
            SetEnemyHealth(target);
        }
    }

    void SetEnemyHealth(GameObject modTarget)
    {
        float multF = Mathf.Min(1 + currentWave / 5, 5);
        int mult = Mathf.RoundToInt(multF);
        modTarget.GetComponent<HealthManager>().maxHealth *= mult;
        modTarget.GetComponent<HealthManager>().healthAmount *= mult;
    }

    void SetBossHealth(GameObject modTarget)
    {
        float multF = Mathf.Min(1 + currentWave / 10, 4);
        int mult = Mathf.RoundToInt(multF);
        modTarget.GetComponent<HealthManager>().maxHealth *= mult;
        modTarget.GetComponent<HealthManager>().healthAmount *= mult;
    }

    void SpawnEnemiesWithBoss()
    {
        GameObject[] enemyArray = GetEnemyArrayForWave();

        int enemiesToSpawn = Mathf.Min(currentWave / 5, 10);

        for (int i = 0; i < enemiesToSpawn; i++)
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
        GameObject target = Instantiate(boss, spawnPoint, Quaternion.identity);
        SetBossHealth(target);
        SpawnEnemiesWithBoss();
    }

    GameObject[] GetEnemyArrayForWave()
    {
        if (currentWave < 2)
        {
            return lowLevelEnemies;
        }
        else if (currentWave < 7)
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



