using System.Collections;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using static PlayerController;

public class Pickup : MonoBehaviour
{

    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private bool shouldMove; 
    [SerializeField] private Animator animator;
    [SerializeField] PickupType pickupType;
    [SerializeField] ProjectileType projectileType;
    private bool usedPickup;
    [Header("Stun Setting")]
    [SerializeField] private float stunTime;

    [Header("Health Setting")]
    [SerializeField] private float healthAmount;

    [Header("Speed Setting")]
    [SerializeField] private float SpeedIncreaseAmount;

    [Header("Damage Setting")]
    [SerializeField] float damageMultIncrease = 1.25f;
    [SerializeField] IncreaseType increaseType;

    private enum IncreaseType
    {
        Additive,
        Multiplicative
    }

    private enum PickupType
    { 
        Hijack,
        Health,
        Speed,
        Damage,
        FireRate,
        ProjectileCount,
        Burst,
        ProjectileSpeed,
        ProjectileType 
    }


    private void Update()
    {
        if(shouldMove)
        {
            MoveDown();
        }
    }

    private void MoveDown()
    {
        transform.Translate(Vector3.down * moveSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!usedPickup && collision.tag == "Player")
        {
            usedPickup = true;
            animator.Play("Pickup");
            //play pickup sound
            ApplyPickup();
        }
    }

    public void ApplyPickup()
    {
        //apply the pickup based on the effect it has
        switch (pickupType)
        {
            case PickupType.Hijack: ApplyHijack(); break;

            case PickupType.Health: ApplyHealth(); break;
               
            case PickupType.Speed: ApplySpeed(); break;

            case PickupType.Damage: ApplyDamage(); break;

            case PickupType.FireRate: ApplyFireRate(); break;

            case PickupType.ProjectileCount: ApplyProjectileCount(); break;

            case PickupType.Burst: ApplyBurst(); break;

            case PickupType.ProjectileSpeed: ApplyProjectileSpeed(); break;

            case PickupType.ProjectileType: ApplyProjectileType(); break;

            default: break;

        }

        Destroy(this.gameObject, 2f);
    }

    #region Applys
    private void ApplyHijack()
    {
        GameObject[] enemyArray = GameObject.FindGameObjectsWithTag("Enemy");
        for(int i = 0; i < enemyArray.Length; i++)
        {
            #region DisableShooter
            Shooter[] instance = enemyArray[i].GetComponents<Shooter>();
            foreach (Shooter shooter in instance)
            {
                shooter.SetStunned(stunTime);
            }
            #endregion

            EnemyController movementInstance = enemyArray[i].GetComponent<EnemyController>();
            movementInstance.SetStunned(stunTime);
        }
    }

    private void ApplyHealth()
    {
        Health healthComp = GetComponent<Health>();
        if(healthComp != null)
        {
            healthComp.AddHealth(healthAmount);
        }
    }

    private void ApplySpeed()
    {
        PlayerController.instance.Speed = SpeedIncreaseAmount;
    }

    private void ApplyDamage()
    {
        if (increaseType == IncreaseType.Additive)
        {
            PlayerController.instance.DamageMult += damageMultIncrease;
        }
        else
        {
            PlayerController.instance.DamageMult *= damageMultIncrease;
        }     
    }

    private void ApplyFireRate()
    {

    }

    private void ApplyProjectileCount()
    {

    }

    private void ApplyBurst()
    {

    }

    private void ApplyProjectileSpeed()
    {

    }
    
    private void ApplyProjectileType()
    {
        PlayerController.instance.ChangeProjectile(projectileType);
    }

    #endregion



}