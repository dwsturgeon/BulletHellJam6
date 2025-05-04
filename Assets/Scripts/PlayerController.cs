using System;
using System.Collections;
using System.Data.Common;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Variables")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float addedRotation;

    [Header("Projectile VARS")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float bulletMoveSpeed;
    [SerializeField] private Transform muzzleTransform;
    [SerializeField] private int burstCount;
    [SerializeField] private int projectilesPerBurst;
    [SerializeField] private float timeBetweenBursts;
    [SerializeField] private float restTime = 1f;
    [SerializeField][Range(0, 359)] private float angleSpread;
    [SerializeField] private float startingDistance = 0.1f;
    private bool isShooting = false;
    private GameObject projectile;
    private Vector2 shootDirection;
    private float angle;

    [Header("Movement Bounds")]
    [SerializeField] private float minX = -10f;
    [SerializeField] private float maxX = 10f;
    [SerializeField] private float minY = -10f;
    [SerializeField] private float maxY = 10f;

    [Header("PlayerTiltVars")]
    [SerializeField] private float maxTiltAngle = 15f;
    [SerializeField] private float tiltSpeed = 5f;
    [SerializeField] private Transform bodyTransform;


    private Rigidbody2D playerRB;
    private Vector2 moveInput;
    private Vector3 mouseWorldPos;
    public static PlayerController instance;


    //add audio for gunshot

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        playerRB = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            Shoot();
        }
    }

    public void Shoot()
    {
        if (!isShooting)
        {
            StartCoroutine(ShootRoutine());
        }
    }

    void FixedUpdate()
    {
        PlayerMovement();
        tiltSprite();
        ClampPosition();
    }

    private void PlayerMovement()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        moveInput = new Vector2(moveX, moveY).normalized;
        playerRB.AddForce(moveInput * moveSpeed);
    }

    private void tiltSprite()
    {
        float horizontalVelocity = playerRB.linearVelocity.x;
        float tiltAmount = Mathf.Clamp(-horizontalVelocity, -1f, 1f);
        float targetZ = tiltAmount * maxTiltAngle;

        Quaternion targetRotation = Quaternion.Euler(0, 0, targetZ);
        bodyTransform.rotation = Quaternion.Lerp(bodyTransform.localRotation, targetRotation, Time.fixedDeltaTime * tiltSpeed);
    }

    private IEnumerator ShootRoutine()
    {
        isShooting = true;

        float startAngle, currentAngle, angleStep, endAngle;

        TargetConeOfInfluence(out startAngle, out currentAngle, out angleStep, out endAngle);

        
        for (int i = 0; i < burstCount; i++)
        {           

            for (int j = 0; j < projectilesPerBurst; j++)
            {
                Vector2 pos = FindBulletSpawnPos(currentAngle);

                GameObject newBullet = Instantiate(projectilePrefab, pos, Quaternion.identity);
                newBullet.transform.right = newBullet.transform.position - transform.position;

                if (newBullet.TryGetComponent(out PlayerProjectile projectile))
                {
                    projectile.UpdateMoveSpeed(bulletMoveSpeed);
                }

                currentAngle += angleStep;
            }

            currentAngle = startAngle;
            yield return new WaitForSeconds(timeBetweenBursts);

        }


        yield return new WaitForSeconds(restTime);
        isShooting = false;

    }

    private void TargetConeOfInfluence(out float startAngle, out float currentAngle, out float angleStep, out float endAngle)
    {
        Vector2 targetDirection = Vector2.up;
        
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

    private void ClampPosition()
    {
        Vector2 pos = playerRB.position;
        Vector2 velocity = playerRB.linearVelocity;

        float clampedX = Mathf.Clamp(pos.x, minX, maxX);
        float clampedY = Mathf.Clamp(pos.y, minY, maxY);

        if (pos.x <= minX && velocity.x < 0) velocity.x = 0;
        if (pos.x >= maxX && velocity.x > 0) velocity.x = 0;

        if (pos.y <= minY && velocity.y < 0) velocity.y = 0;
        if (pos.y >= maxY && velocity.y > 0) velocity.y = 0;

        playerRB.position = new Vector2(clampedX, clampedY);
        playerRB.linearVelocity = velocity;

    }
}
