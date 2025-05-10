using System.Collections;
using System.Data;
using System.Runtime.CompilerServices;
using System.Security;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UIElements;

public class Shooter : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletMoveSpeed;
    [SerializeField] private int burstCount;
    [SerializeField] private int projectilesPerBurst;
    [SerializeField][Range(0, 359)] private float angleSpread;
    [SerializeField] private float startingDistance = 0.1f;
    [SerializeField] private float timeBetweenBursts;
    [SerializeField] private float restTime = 1f;
    [SerializeField] private bool stagger;
    [SerializeField] private bool oscillate;
    [SerializeField] private bool shouldWaitToStop = false;
    [SerializeField] private Transform muzzleTransform;

    private bool stunned = false;
    private float stunTimer;
    [SerializeField] private float recoverMult = 1f;
    [SerializeField] Animator animator;
    [SerializeField] private bool useAnimator;

    [Header("Anomaly Zone")]
    [SerializeField] private bool bUseZone = false;
    [SerializeField] GameObject zone;
    [SerializeField] float zoneCooldown = 1f;
    [SerializeField] float zoneLifetime = 1f;
    private float elapsed;
    private enum Orientation
    {
        Left, Right, Up, Down, AtPlayer,
    }

    [SerializeField] private Orientation orientation;
    EnemyController movementComp;



    private bool isShooting = false;
    private void Start()
    {
        movementComp = GetComponent<EnemyController>();
        if(muzzleTransform == null ) muzzleTransform.position = transform.position;
        
    }

    private void Update()
    {
        if (stunned)
        {
            if (isShooting)
            {
                StopAllCoroutines();
                isShooting=false;
            }

            if (stunTimer >= 0f)
            {
                stunTimer -= (recoverMult * Time.deltaTime);
            }
            else
            {
                stunned = false;
            }

        }
        else Shoot();

        
    }

    public void Shoot()
    {
        if(shouldWaitToStop)
        {
            if (!isShooting && movementComp.GetWaitStatus())
            {
                StartCoroutine(ShootRoutine());
            }
        }
        else
        {
            if (!isShooting)
            {
                StartCoroutine(ShootRoutine());
            }
        }

        if (bUseZone)
        {
            elapsed += Time.deltaTime;

            if (elapsed > zoneCooldown)
            {
                SpawnZone();
                elapsed = 0;
            }
        }
    }

    private IEnumerator ShootRoutine()
    {
        isShooting = true;

        float startAngle, currentAngle, angleStep, endAngle;
        float timeBetweenProjectiles = 0f;

        TargetConeOfInfluence(out startAngle, out currentAngle, out angleStep, out endAngle);

        if (stagger) timeBetweenProjectiles = timeBetweenBursts / projectilesPerBurst;
       

        for (int i = 0; i < burstCount; i++)
        {
            if (useAnimator) animator.SetBool("bAttack", true);
            

            

            if(!oscillate)
            {
                TargetConeOfInfluence(out startAngle, out currentAngle, out angleStep, out endAngle);
            }
            if (oscillate && i % 2 != 1) 
            {
                TargetConeOfInfluence(out startAngle, out currentAngle, out angleStep, out endAngle);
            }
            else if (oscillate)
            {
                currentAngle = endAngle;
                endAngle = startAngle;
                startAngle = currentAngle;
                angleStep *= -1;
            }


            for (int j = 0; j < projectilesPerBurst; j++)
            {
                Vector2 pos = FindBulletSpawnPos(currentAngle);

                GameObject newBullet = Instantiate(bulletPrefab, pos, Quaternion.identity);
                newBullet.transform.right = newBullet.transform.position - muzzleTransform.position;

                if (newBullet.tag == "BulletContainer")
                {
                    ContainerSpawner logic = newBullet.GetComponent<ContainerSpawner>();
                    logic.spawner = this.transform;
                }
                else
                {
                    FadeByDistance logic = newBullet.GetComponent<FadeByDistance>();
                    logic.spawner = this.transform;
                }


                if (newBullet.TryGetComponent(out Projectile projectile))
                {
                    projectile.UpdateMoveSpeed(bulletMoveSpeed);
                }

                currentAngle += angleStep;

                if (stagger) yield return new WaitForSeconds(timeBetweenProjectiles);
            }

            currentAngle = startAngle;
            if (useAnimator) animator.SetBool("bAttack", false);
            yield return new WaitForSeconds(timeBetweenBursts);
            
        }


        yield return new WaitForSeconds(restTime);
        isShooting = false;

    }

    private void TargetConeOfInfluence(out float startAngle, out float currentAngle, out float angleStep, out float endAngle)
    {
        Vector2 targetDirection;
        switch (orientation)
        {
            case Orientation.Left: targetDirection = Vector2.left; break;

            case Orientation.Right: targetDirection = Vector2.right; break;

            case Orientation.Up: targetDirection = Vector2.up; break;

            case Orientation.Down: targetDirection = Vector2.down; break;

            default: targetDirection = PlayerController.instance.transform.position - muzzleTransform.position; break;
        }



        float targetAngle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
        startAngle = targetAngle;
        endAngle = targetAngle;
        currentAngle = targetAngle;
        float halfAngleSpread = 0f;
        angleStep = 1f;
        if (angleSpread != 0)
        {
            angleStep = angleSpread / (projectilesPerBurst - 1);
            halfAngleSpread = angleSpread / 2f;
            startAngle = targetAngle - halfAngleSpread;
            endAngle = targetAngle + halfAngleSpread;
            currentAngle = startAngle;
        }
    }

    private Vector2 FindBulletSpawnPos(float currentAngle)
    {
        
        float x = muzzleTransform.position.x + startingDistance * Mathf.Cos(currentAngle * Mathf.Deg2Rad);
        float y = muzzleTransform.position.y + startingDistance * Mathf.Sin(currentAngle * Mathf.Deg2Rad);
        
        
        Vector2 pos = new Vector2(x, y);
        return pos;
    }

    public void SetStunned(float _stunT)
    {
        stunTimer = _stunT;
        stunned = true;
    }

    private void SpawnZone()
    {
        Vector3 centerPosition = new Vector3(0f, 0f, 0f);

        float influenceFactor = 0.1f;
        centerPosition.x += PlayerController.instance.transform.position.x * influenceFactor;

        System.Array values = System.Enum.GetValues(typeof(AnomalyZone.ZoneType));
        AnomalyZone.ZoneType randomValue = (AnomalyZone.ZoneType)values.GetValue(UnityEngine.Random.Range(0, values.Length));
        GameObject spawnedZone = Instantiate(zone, centerPosition, Quaternion.identity);
        spawnedZone.GetComponent<AnomalyZone>().Zone = randomValue;
        spawnedZone.GetComponent<FadeIn>().lifetime = zoneLifetime;


        
        
        Destroy(spawnedZone, zoneLifetime);
        
    }
}
