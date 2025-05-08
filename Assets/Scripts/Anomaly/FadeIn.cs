using UnityEngine;
using UnityEngine.U2D;

public class FadeIn : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    [SerializeField] private float speed = 1f;
    [SerializeField] private float maxAlpha = 0.25f;

    private void Update()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        Color color = spriteRenderer.color;

        color.a = Mathf.Clamp(color.a + speed * Time.deltaTime, 0, maxAlpha);
        spriteRenderer.color = color;
        if(color.a == maxAlpha) Destroy(this);
    }
}
