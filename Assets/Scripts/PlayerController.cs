using System;
using System.Data.Common;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Variables")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float addedRotation;
    [SerializeField] private bool wantsRecoil = false;

    [Header("Projectile VARS")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform muzzleTransform;
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
        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
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

    private void Shoot()
    {
        shootDirection = transform.up;

        projectile = Instantiate(projectilePrefab, muzzleTransform.position, Quaternion.identity);

        projectile.GetComponent<PlayerProjectile>().SetDirection(shootDirection);

        angle = Mathf.Atan2(shootDirection.y, shootDirection.x) * Mathf.Rad2Deg + addedRotation;
        projectile.transform.rotation = Quaternion.Euler(0f, 0f, angle);
        if(wantsRecoil) playerRB.AddForce(-shootDirection * .5f, ForceMode2D.Impulse);

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
