using UnityEditor;
using UnityEngine;

public class AnomalyFade : MonoBehaviour
{
    [SerializeField] Material dissolveMat;
    
    [SerializeField] float maxDissolve;

    float maxHp;
    float currentHp;
    HealthManager healthComp;

    private void Start()
    {
        healthComp = this.GetComponent<HealthManager>();
        maxHp = healthComp.maxHealth;    
    }

    private void Update()
    {
        float healthPercentage = Mathf.Clamp01(healthComp.healthAmount / maxHp);
        float dissolveValue = Mathf.Lerp(maxDissolve, 0f, healthPercentage);
        dissolveMat.SetFloat("_DissolveAmount", dissolveValue);
    }
}
