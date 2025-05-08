using System;
using System.Collections;
using System.Data.Common;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class HealthManager : MonoBehaviour
{

    public Image healthBar;
    public float healthAmount = 100f;
    public float maxHealth = 100f;
    public float gracePeriod = .1f;
    private float elapsed;
    [SerializeField] private bool isBoss = false;

    [SerializeField] private string targetTag = "PlayerProjectile";

    [SerializeField] GameObject[] drops;
    //[SerializeField] private GameObject enemy;

    public static HealthManager instance;

    private CircleCollider2D enemyCollider;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        enemyCollider = GetComponent<CircleCollider2D>();
    }
    // Update is called once per frame
    void Update()
    {
        elapsed += Time.deltaTime;
        if (healthAmount <= 0)
        {
            SpawnRandomDrop();
            Destroy(gameObject);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {

        if (collision.CompareTag(targetTag))
        {
            if(elapsed >= gracePeriod)
            {
                TakeDamage(collision.GetComponent<PlayerProjectile>().Damage);
                elapsed = 0;
            }
                
        }

    }

    public void TakeDamage(float damage)
    {
        healthAmount -= damage;
        healthBar.fillAmount = healthAmount / maxHealth;

    }

    public void Heal(float healingAmount)
    {
        healthAmount += healingAmount;
        healthAmount = Mathf.Clamp(healthAmount, 0, maxHealth);
        healthBar.fillAmount = healthAmount / maxHealth;
    }

    private void SpawnRandomDrop()
    {
        int dropNum = Random.Range(0, drops.Length - 1);
        if (isBoss)
        {
            GameObject drop = Instantiate(drops[dropNum], transform.position, Quaternion.identity);
            Pickup pickup = drop.GetComponent<Pickup>();
            pickup.Boss = true;
        }
        else Instantiate(drops[dropNum], transform.position, Quaternion.identity);
    }
}
