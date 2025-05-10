using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;


public class Health : MonoBehaviour
{
    [SerializeField] private float startingHealth;
    [SerializeField] private string targetTag = "Projectile";
    [SerializeField] private Slider HealthBar;
    private float currentHealth;
    private float elapsed;
    private bool isDead = false;
    [SerializeField] Animator animator;
    [SerializeField] private float gracePeriod = 0.2f;
    [SerializeField] private bool bGodMode = false;
    [SerializeField] private TextMeshProUGUI HealthUpgadeCountText;
    private int healthUpgadeCount;
    public bool isGlitching = false;


    [SerializeField] GameObject deathScreen;

    [Header("DeathSound")]
    [SerializeField] AudioSource DeathAudio;
    [SerializeField] AudioSource DeathScreenSound;
    [SerializeField] AudioSource HurtAudio;
    private bool playedDeathScreenSound = false;




    private int healthUpgradeCount = 0;


    private void Awake()
    {
        currentHealth = startingHealth;
        HealthBar.maxValue = startingHealth;

    }

    private void Start()
    {
        UpdateHealthUI(currentHealth);
    }

    private void FixedUpdate()
    {
        elapsed += Time.fixedDeltaTime;
        if(isDead) 
        {
            if (!playedDeathScreenSound && elapsed >= 0.3f)
            {
                DeathScreenSound.Play();
                playedDeathScreenSound = true;
            }
            Time.timeScale = Mathf.Clamp(Time.timeScale - Time.deltaTime, 0.1f, 1f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (isGlitching) return;

        if (collision.CompareTag(targetTag))
        {
            if (elapsed >= gracePeriod)
            {
                if (!bGodMode)
                {
                    RemoveHealth(collision.GetComponent<Projectile>().Damage);
            
                    elapsed = 0;
                }

            }
            

        }

    }

    public void RemoveHealth(float amount)
    {
        currentHealth = Mathf.Clamp(currentHealth - amount, 0, startingHealth);
        UpdateHealthUI(currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }

    }

    public void AddHealth(float amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, startingHealth);
        healthUpgradeCount++;
        UpdateHealthUI(currentHealth);
    }

    public void AddMaxHealth(float amount)
    {
        startingHealth = Mathf.Clamp(startingHealth + amount, 0, 300);
        healthUpgadeCount++;
        HealthUpgadeCountText.text = healthUpgadeCount.ToString();
        UpdateHealthMaxUI(startingHealth);

    }

    private void UpdateHealthUI(float healthValue)
    {
        HealthBar.value = healthValue;
    }

    private void UpdateHealthMaxUI(float value)
    {
        HealthBar.maxValue = value;
    }

    public int HealthUpgradeC { get => healthUpgradeCount; }


    private void Die()
    {
        if (!isDead)
        {
            PlayDeathSound();
            GetComponent<PlayerController>().enabled = false;
            GetComponent<GlitchAbility>().enabled = false;
            GetComponent<PlayerController>().isDead = true;
            animator.SetBool("Dead", true);

            isDead = true;
            elapsed = 0;

            GameObject.Find("Canvas").GetComponent<SceneFunctions>().settingsUseable = false;
            deathScreen.SetActive(true);
        } 
    }

    public void PlayDeathSound()
    {
        DeathAudio.Play();
    }
    
}
