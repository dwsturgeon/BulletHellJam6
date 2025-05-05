using Unity.VisualScripting;
using UnityEngine;

public class Pickup : MonoBehaviour
{

    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private Animator animator;
    [SerializeField] PickupType pickupType;
    [SerializeField] private float stunTime;
    private bool usedPickup;


    private enum PickupType
    { 
        Hijack,
        Speed,
        Damage,
        FireRate,
        ProjectileCount,
        Burst,
        ProjectileSpeed       
    }


    private void Update()
    {
        MoveDown();
    }

    private void MoveDown()
    {
        transform.Translate(Vector3.down * moveSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!usedPickup)
        {
            //usedPickup = true;
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

            default: break;
                
        }

        Destroy(this.gameObject, 2f);
    }

    #region Apply's
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

    private void ApplySpeed()
    {

    }

    private void ApplyDamage()
    {

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

    #endregion



}