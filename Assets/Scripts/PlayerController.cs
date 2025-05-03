using System;
using System.Data.Common;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Variables")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float addedRotation;

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
        rotatePlayerTowardsMouse();
        ClampPosition();
    }

    private void PlayerMovement()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        moveInput = new Vector2(moveX, moveY).normalized;
        playerRB.AddForce(moveInput * moveSpeed);
    }

    private void rotatePlayerTowardsMouse()
    {
        mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f;
        Vector2 direction = (mouseWorldPos - transform.position).normalized;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + addedRotation;
        Quaternion targetRotation = Quaternion.Euler(0f, 0f, angle);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed);
    }

    private void Shoot()
    {
        shootDirection = transform.up;

        projectile = Instantiate(projectilePrefab, muzzleTransform.position, Quaternion.identity);

        projectile.GetComponent<PlayerProjectile>().SetDirection(shootDirection);

        angle = Mathf.Atan2(shootDirection.y, shootDirection.x) * Mathf.Rad2Deg + addedRotation;
        projectile.transform.rotation = Quaternion.Euler(0f, 0f, angle);
        playerRB.AddForce(-shootDirection * .5f, ForceMode2D.Impulse);
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
