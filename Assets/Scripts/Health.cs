using TMPro;
using UnityEngine;
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
        if(isDead && elapsed > 1f ) 
        {
            this.gameObject.SetActive(false);
            Time.timeScale = 0f;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {


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
        startingHealth += amount;
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
        GetComponent<PlayerController>().enabled = false;
        GetComponent<GlitchAbility>().enabled = false;
        GetComponent<PlayerController>().isDead = true;
        animator.SetBool("Dead", true);

        isDead = true;
        elapsed = 0;
        //play death sound

    }
    
}
