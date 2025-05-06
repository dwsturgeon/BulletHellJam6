using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float startingHealth;
    private float currentHealth;

    private void Awake()
    {
        currentHealth = startingHealth;
    }

    private void Start()
    {
        UpdateHealthUI(currentHealth);
    }

    public void RemoveHealth(float amount)
    {
        currentHealth = Mathf.Clamp(currentHealth - amount, 0, startingHealth);
        UpdateHealthUI(currentHealth);

        if (currentHealth <= 0)
        {
            //kill player
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
    }


}
