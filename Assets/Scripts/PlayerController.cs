using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using Random = UnityEngine.Random;
using UnityEngine.Audio;
using TMPro;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("Variables")]

    [Header("Move Settings")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float moveSpeedMax;
    [SerializeField] private float addedRotation;

    [Header("Damage Settings")]
    [SerializeField] private float damageMult = 1f;
    [SerializeField] private float damageMultMax = 4f;
    [SerializeField] private float damageMultMin = 1f;

    [Header("Projectile VARS")]
    [Header("Bullet Move Settings")]
    [SerializeField] private float bulletMoveSpeed;
    [SerializeField] private float bulletMoveSpeedMax = 40f;
    [SerializeField] private float bulletMoveSpeedMin = 10f;
    public GameObject laser;

    [Header("Burst Settings")]
    [SerializeField] private int burstCount;
    [SerializeField] private int burstCountMax;
    [SerializeField] private int projectilesPerBurst;
    [SerializeField] private int projectilesPerBurstMax;
    [SerializeField] private float timeBetweenBursts;

    [Header("Rest Settings")]
    [SerializeField] private float restTime = 1f;
    [SerializeField] private float minRestTime = 0.01f;
    [SerializeField] private float maxRestTime = 1f;

    [Header("Spawning Settings")]
    [SerializeField] ProjectileType currentProjectileType;
    [SerializeField][Range(0, 359)] private float angleSpread;
    [SerializeField] private float angleIncreasePerProjectile;
    [SerializeField] private Transform muzzleTransform;
    [SerializeField] private float startingDistance = 0.1f;
    private bool isShooting = false;

    


    [Header("Movement Bounds")]
    [SerializeField] private float minX = -10f;
    [SerializeField] private float maxX = 10f;
    [SerializeField] private float minY = -10f;
    [SerializeField] private float maxY = 10f;

    [Header("PlayerTiltVars")]
    [SerializeField] private float maxTiltAngle = 15f;
    [SerializeField] private float tiltSpeed = 5f;
    [SerializeField] private Transform bodyTransform;

    [Header("Gun Sound Effect")]
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private AudioClip basicClip;
    [SerializeField] private AudioClip explosiveClip;
    [SerializeField] private AudioClip laserClip;
    [SerializeField] private AudioClip crescentClip;
    [SerializeField] private AudioSource gunSound;

    [Header("Pickup Sound")]
    [SerializeField] private AudioSource pickupSource;

    [Header("UI")]

    [SerializeField] private TextMeshProUGUI damageUpgradeCountText;
    [SerializeField] private TextMeshProUGUI firerateUpgradeCountText;
    [SerializeField] private TextMeshProUGUI projectileUpgradeCountText;
    [SerializeField] private TextMeshProUGUI speedUpgradeCountText;
    [SerializeField] private Image Image;
    [SerializeField] private Sprite basicSprite;
    [SerializeField] private Sprite laserSprite;
    [SerializeField] private Sprite crescentSprite;
    [SerializeField] private Sprite explosiveSprite;

    private int DamageUpgradeCount = 0;
    private int FirerateUpgradeCount = 0;
    private int ProjectileUpgradeCount = 0;
    private int SpeedUpgradeCount = 0;
    private int hasBestProjectile = 0;
    private int projIndex = 0;
    public bool isDead = false;
    public bool isPaused = false;



    private AudioSource gunAudioSource;

    private Collider2D thisCollider;
    private Rigidbody2D playerRB;
    private Vector2 moveInput;
    public static PlayerController instance;

    


    #region Projectile Changing

    public enum ProjectileType
    {
        Normal,
        Explosive,
        Laser,
        Crescent
    }

    [System.Serializable]
    private struct ProjectileEntry 
    {
        public ProjectileType type;
        public GameObject prefab;
    }
    [Header("Projectile Types")]
    [SerializeField] private ProjectileEntry[] projectileEntries;

    private Dictionary<ProjectileType, GameObject> projectileMap;


    public void ChangeProjectile(ProjectileType newType)
    {
        if (projectileMap.ContainsKey(newType))
        {
            if (laser != null) 
            {
                Destroy(laser);
            }

            currentProjectileType = newType;
            ChangeProjecileUI();
        }
    }

    private void ChangeProjecileUI()
    {
        switch (currentProjectileType)
        {
            case ProjectileType.Normal: Image.sprite = basicSprite; break;

            case ProjectileType.Laser: Image.sprite = laserSprite; break;

            case ProjectileType.Crescent: Image.sprite = crescentSprite; break;

            case ProjectileType.Explosive: Image.sprite = explosiveSprite; break;
        }

    }
    #endregion

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
        thisCollider = GetComponent<Collider2D>();

        projectileMap = new Dictionary<ProjectileType, GameObject>();
        foreach (var entry in projectileEntries)
        {
            projectileMap[entry.type] = entry.prefab;
        }

        if(ProjectileCount == 1)
        {
            Angle = 0;
        }

    }

    private void Update()
    {
        if (currentProjectileType == ProjectileType.Laser)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if(Input.GetKeyDown(KeyCode.Space) && laser == null)
                {
                    if (projectileMap.TryGetValue(currentProjectileType, out var prefab))
                    {
                        laser = Instantiate(prefab, muzzleTransform.position, transform.rotation, transform);
                        Physics2D.IgnoreCollision(laser.GetComponent<Collider2D>(), thisCollider);
                    }
                }
            }
            else if (Input.GetKeyUp(KeyCode.Space) && laser != null)
            {
                Destroy(laser);
                laser = null;
            }
            
        }
        else if (Input.GetKey(KeyCode.Space))
        {
            Shoot();
        }

    }

    public void Shoot()
    {
        if (!isShooting && !isPaused)
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
        playerRB.AddForce(moveInput * moveSpeed * 100 * Time.deltaTime);
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
                if(projectileMap.TryGetValue(currentProjectileType, out GameObject projectilePrefab))
                {

                    GameObject newBullet = Instantiate(projectilePrefab, pos, Quaternion.identity);
                    PlayGunSound(gunSound);
                    
                    PlayerProjectile bulletDamage = newBullet.GetComponent<PlayerProjectile>();
                    bulletDamage.Damage = bulletDamage.Damage * damageMult;

                    newBullet.transform.right = newBullet.transform.position - muzzleTransform.position;

                    if (newBullet.TryGetComponent(out PlayerProjectile projectile))
                    {
                        projectile.UpdateMoveSpeed(bulletMoveSpeed);
                    }

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

    public void PlayPickupSound()
    {
        pickupSource.pitch = Random.Range(1f, 1.2f);
        pickupSource.Play();
    }

    private void PlayGunSound(AudioSource audio)
    {
        switch (currentProjectileType) 
        {

            case ProjectileType.Normal: 
            {
                audio.clip = basicClip;
                audio.volume = 1f;
                break;
            }

            case ProjectileType.Crescent:
            { 
                audio.clip = crescentClip;
                audio.volume = 0.4f;
                break;
            }

            case ProjectileType.Explosive:
            {
                audio.clip = explosiveClip;
                audio.volume = 0.15f;
                break;
            }

            case ProjectileType.Laser:  
            {
                audio.clip = laserClip;
                audio.volume = 1f;
                break;
            }

            default: break;
        }

        audio.pitch = Random.Range(0.95f, 1.05f);
        audio.Play();
    }

    #region Var SET GET
    public float Speed 
    {
        get => moveSpeed;
        set
        {
            moveSpeed = Mathf.Clamp(moveSpeed + value, 0, moveSpeedMax);
            SpeedUpgradeCount++;
            speedUpgradeCountText.text = SpeedUpgradeCount.ToString();
        }
    }

    public float DamageMult
    {
        get => damageMult;
        set 
        {
            damageMult = Mathf.Clamp(value, damageMultMin, damageMultMax);
            DamageUpgradeCount++;
            damageUpgradeCountText.text = DamageUpgradeCount.ToString();
        }
    }

    public float FireRate 
    { 
        get => restTime;
        set 
        {
            restTime = Mathf.Clamp(value, minRestTime, maxRestTime);
            FirerateUpgradeCount++;
            firerateUpgradeCountText.text = FirerateUpgradeCount.ToString();
        } 

    }

    public int ProjectileCount
    {
        get => projectilesPerBurst;
        set 
        {
            projectilesPerBurst = Mathf.Clamp(value, 1, projectilesPerBurstMax);
            if(projectilesPerBurst == 1)
            {
                Angle = 0;
            }
            else
            {
                Angle = ProjectileCount * AnglePerProj;
            }
            ProjectileUpgradeCount++;
            projectileUpgradeCountText.text = ProjectileUpgradeCount.ToString();

        }
    }

    public float Angle
    {
        get => angleSpread;
        set
        {
            angleSpread = Mathf.Clamp(value, 0f, 80f);
        }
    }

    public float AnglePerProj //if we ever think it should be an upgrade
    {
        get => angleIncreasePerProjectile;
        set { angleIncreasePerProjectile = Mathf.Clamp(value, 0f, 20f); }
    }

    public int BurstCount
    {
        get => burstCount;
        set { burstCount = Mathf.Clamp(value, 0, burstCountMax); }
    }

    public float ProjectileSpeed
    {
        get => bulletMoveSpeed;
        set { bulletMoveSpeed = Mathf.Clamp(value, bulletMoveSpeedMin, bulletMoveSpeedMax); }
    }
    #endregion

    #region UpgradeCount Get 

    public int DamageUpgadeC { get => DamageUpgradeCount; }
    public int FirerateUpgadeC { get => FirerateUpgradeCount; }
    public int SpeedUpgadeC { get => SpeedUpgradeCount; }
    public int ProjectileUpgadeC { get => ProjectileUpgradeCount; }

    public int BestProj 
    {
        get => hasBestProjectile;
        set => hasBestProjectile = value;      
    }

    public int ProjectileIndex { get => projIndex; set => projIndex = value; }
    #endregion


}