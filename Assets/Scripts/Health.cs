using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float startingHealth;
    [SerializeField] private string targetTag = "Projectile";
    private float currentHealth;

    private void Awake()
    {
        currentHealth = startingHealth;
    }

    private void Start()
    {
        UpdateHealthUI(currentHealth);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag(targetTag))
        {
            /*if (elapsed >= gracePeriod)
            {*/
                RemoveHealth(collision.GetComponent<Projectile>().Damage);
            /*
                elapsed = 0;
             }
            */

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
        UpdateHealthUI(currentHealth);
    }

    private void UpdateHealthUI(float healthValue)
    {
        //update health on UI
        //wip
    }


}
