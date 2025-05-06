using System;
using System.Collections;
using System.Data.Common;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{

    public Image healthBar;
    public float healthAmount = 100f;
    public float maxHealth = 100f;

    [SerializeField] private string targetTag = "PlayerProjectile";
    [SerializeField] private GameObject enemy;

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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (healthAmount <= 0)
        {
            Destroy(enemy);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(targetTag))
        {
            TakeDamage(collision.GetComponent<PlayerProjectile>().Damage);
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
}
