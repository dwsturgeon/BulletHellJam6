using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
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

    private Collider2D thisCollider;

    private enum Orientation
    {
        Left, Right, Up, Down, AtPlayer,
    }

    [SerializeField] private Orientation orientation;
    EnemyMovement movementComp;


    private bool isShooting = false;
    private void Start()
    {
        thisCollider = GetComponent<Collider2D>();
        movementComp = GetComponent<EnemyMovement>();
    }

    private void Update()
    {
        Shoot();
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
            if(!oscillate)
            {
                TargetConeOfInfluence(out startAngle, out currentAngle, out angleStep, out endAngle);
            }
            if (oscillate && i % 2 != 1 && timeBetweenBursts !> 0.5f) 
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
                Physics2D.IgnoreCollision(newBullet.GetComponent<Collider2D>(), thisCollider);
                newBullet.transform.right = newBullet.transform.position - transform.position;

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

            default: targetDirection = PlayerController.instance.transform.position - transform.position; break;
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
        float x = transform.position.x + startingDistance * Mathf.Cos(currentAngle * Mathf.Deg2Rad);
        float y = transform.position.y + startingDistance * Mathf.Sin(currentAngle * Mathf.Deg2Rad);

        Vector2 pos = new Vector2(x, y);
        return pos;
    }

}
