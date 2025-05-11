using System;
using System.Collections;
using System.Collections.Generic;
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
    private bool isDead = false;
    [SerializeField] private bool isBoss = false;

    [SerializeField] private string targetTag = "PlayerProjectile";

    private GameObject[] drops;
    private List<GameObject> dropProjectiles;
    [SerializeField] private DropConfig dropConfig;
    [SerializeField] private DropProjectileConfig dropProjectileConfig;


    [SerializeField] private AudioSource HurtSource;
    [SerializeField] private AudioSource DeathSource;


    public static HealthManager instance;

    private CircleCollider2D enemyCollider;

    private void Awake()
    {
        if(dropConfig == null || dropProjectileConfig == null)
        {
            Debug.Log("Add Config In Inspector");
        }
        drops = dropConfig.Drops;
        dropProjectiles = new List<GameObject>(dropProjectileConfig.dropProjectiles);

        if (instance == null)
        {
            instance = this;
        }

        enemyCollider = GetComponent<CircleCollider2D>();
    }

    void Update()
    {
        elapsed += Time.deltaTime;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {

        if (collision.gameObject.GetComponent<PlayerProjectile>().ProjType == PlayerProjectile.ProjectileType.Explosive)
        {
            collision.GetComponent<PlayerProjectile>().Explode();
        }

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
        if (healthAmount <= 0)
        {
            Die();
        }
        else PlayHurtSound();
    }

    public void Heal(float healingAmount)
    {
        healthAmount += healingAmount;
        healthAmount = Mathf.Clamp(healthAmount, 0, maxHealth);
        healthBar.fillAmount = healthAmount / maxHealth;
    }

    private void SpawnRandomDrop()
    {
        int dropNum;
        if(PlayerController.instance.BestProj == 0)
        {
            dropNum = Random.Range(0, drops.Length + 1);
        }
        else
        {
            dropNum = Random.Range(0, drops.Length);
        }

        if (dropNum == drops.Length)
        {

            GameObject drop = Instantiate(dropProjectiles[PlayerController.instance.ProjectileIndex], transform.position, Quaternion.identity);
            PlayerController.instance.ProjectileIndex += 1;
            if (PlayerController.instance.ProjectileIndex > 2)
            {
                PlayerController.instance.BestProj = 1;
            }



        }
        else
        {

            GameObject drop = Instantiate(drops[dropNum], transform.position, Quaternion.identity);
            Pickup pickup = drop.GetComponent<Pickup>();

            if (isBoss) pickup.Boss = true;
            else pickup.Boss = false;
        }
    }

    void Die()
    {
        if(!isDead)
        {
            PlayDeathSound();
            SpawnRandomDrop();


            FadeFromDamage fadeFromDamage = GetComponent<FadeFromDamage>();
            fadeFromDamage.isDying = true;
            Destroy(this.gameObject, 2.5f);
            isDead = true;

            //PLAY DEATH SOUND
   
            Shooter[] shooters = GetComponents<Shooter>();
            for(int i = 0; i < shooters.Length; i++)
            {
                shooters[i].enabled = false;
                shooters[i].StopAllCoroutines();
            }

            GetComponent<Collider2D>().enabled = false;
            GetComponent<Shooter>().enabled = false;
            GetComponent<EnemyController>().enabled = false;
            
        }
    }

    private void PlayDeathSound()
    {
        DeathSource.pitch = Random.Range(1f, 1.2f);
        DeathSource.Play();
    }

    private void PlayHurtSound()
    {
        if (HurtSource.isPlaying)
        {
            HurtSource.Stop();
        }
        HurtSource.pitch = Random.Range(1f, 1.2f);
        HurtSource.Play();
    }

    
}
