using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [SerializeField] private float startingHealth;
    [SerializeField] private string targetTag = "Projectile";
    [SerializeField] private Slider HealthBar;
    private float currentHealth;
    private float elapsed;
    [SerializeField] private float gracePeriod = 0.2f;
    [SerializeField] private bool bGodMode = false;

    private int healthUpgradeCount = 0;

    private void Awake()
    {
        currentHealth = startingHealth;
    }

    private void Start()
    {
        UpdateHealthUI(currentHealth);
    }

    private void FixedUpdate()
    {
        elapsed += Time.fixedDeltaTime;
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
                    Debug.Log(currentHealth);
            
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
            //for now
            gameObject.SetActive(false);
        }

    }

    public void AddHealth(float amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, startingHealth);
        healthUpgradeCount++;
        UpdateHealthUI(currentHealth);
    }

    private void UpdateHealthUI(float healthValue)
    {
        //HealthBar.value = healthValue;
    }

    public int HealthUpgradeC { get => healthUpgradeCount; }

}
