using UnityEditor;
using UnityEngine;

public class FadeFromDamage : MonoBehaviour
{
    [SerializeField] private Material dissolveMat;
    [SerializeField] float maxDissolve = .65f;
    [SerializeField] SpriteRenderer visualTarget;

    private Material dissolveMatInstance;
    float maxHp;
    HealthManager healthComp;
    private float maxDieDissolve = 1.1f;


    public bool isDying = false;
    

    private void Start()
    {
        healthComp = this.GetComponent<HealthManager>();
        maxHp = healthComp.maxHealth; 
        
        if(visualTarget == null) Debug.Log("Assign the mat for " + this.gameObject);   

        dissolveMatInstance = new Material(dissolveMat);
        visualTarget.material = dissolveMatInstance;        
    }

    private void Update()
    {
        if (!isDying)
        {
            float healthPercentage = Mathf.Clamp01(healthComp.healthAmount / maxHp);
            float dissolveValue = Mathf.Lerp(maxDissolve, 0f, healthPercentage);
            dissolveMatInstance.SetFloat("_DissolveAmount", dissolveValue);
        }
        else
        {
            float currentDissolve = dissolveMatInstance.GetFloat("_DissolveAmount");
            float newDissolve = Mathf.MoveTowards(currentDissolve, maxDieDissolve, 0.4f *Time.deltaTime);
            dissolveMatInstance.SetFloat("_DissolveAmount", newDissolve);
        }
    }  
}
