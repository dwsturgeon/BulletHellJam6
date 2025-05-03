using System.Collections;
using UnityEngine;

public class FlashOnCollision : MonoBehaviour
{

    private SpriteRenderer sp;
    private Color originalColor;

    private Coroutine flashCoroutine;

    [SerializeField] private Color flashColor;
    [SerializeField] private float fadeDuration= 0.1f;
    [SerializeField] private string targetTag = "PlayerProjectile";
    [SerializeField] private GameObject visualTarget;

    private void Start()
    {
        sp = visualTarget.GetComponent<SpriteRenderer>();
        originalColor = sp.color;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(targetTag))
        {
            if(flashCoroutine != null)
            {
                StopCoroutine(flashCoroutine);
                sp.color = flashColor;
            }
            flashCoroutine = StartCoroutine(Flash());
        }
        
    }

    IEnumerator Flash()
    {
        sp.color = flashColor;
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / fadeDuration;
            sp.color = Color.Lerp(flashColor, originalColor, t);
            yield return null;
        }

    }
}
