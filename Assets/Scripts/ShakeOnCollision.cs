using UnityEngine;
using System.Collections;

public class ShakeOnCollision : MonoBehaviour
{
    [SerializeField] private string targetTag = "PlayerProjectile";
    [SerializeField] private float shakeDuration = 0.5f;
    [SerializeField] private float shakeMagnitude = 0.05f;

    [SerializeField] Transform visualTarget;

    private Vector3 originalPosition;
    private Coroutine shakeCoroutine;

    private void Start()
    {
        originalPosition = visualTarget.localPosition;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(targetTag))
        {
            if(shakeCoroutine != null) StopCoroutine(shakeCoroutine);

            shakeCoroutine = StartCoroutine(Shake());

        }
    }

    IEnumerator Shake()
    {
        float elapsed = 0f;
        while (elapsed < shakeDuration) 
        {
            elapsed += Time.deltaTime;

            float offsetX = Random.Range(-1f, 1f) * shakeMagnitude;
            float offsety = Random.Range(-1f, 1f) * shakeMagnitude;

            visualTarget.localPosition = originalPosition + new Vector3(offsetX, offsety, 0f);
            yield return null;
        }

        visualTarget.localPosition = originalPosition;
        shakeCoroutine = null;
    }
}
