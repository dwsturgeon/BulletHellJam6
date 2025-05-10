using UnityEngine;


public class FadeIn : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    [SerializeField] private float maxAlpha = 0.25f;
    public float lifetime = 2f;

    private float elapsed = 0;
    float halfLife;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();   
        halfLife = lifetime / 2;
    }
    private void Update()
    {
        elapsed += Time.deltaTime;
        Color color = spriteRenderer.color;
        float alpha;

        if (elapsed <= halfLife)
        {
            alpha = Mathf.Lerp(0f, maxAlpha, elapsed / halfLife);
        }
        else if(elapsed <= lifetime)
        {
            alpha = Mathf.Lerp(maxAlpha, 0f, (elapsed - halfLife) / halfLife);
        }
        else
        {
            return;
        }
            

        color.a = alpha;
        spriteRenderer.color = color;
    }
}
