using UnityEngine;

public class FadeByDistance : MonoBehaviour
{
    public Transform spawner;
    [SerializeField] private float maxFadeDistance = 1f;
    private SpriteRenderer sr;
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (spawner != null)
        {
            float distance = Vector2.Distance(transform.position, spawner.position);

            float alpha = Mathf.Clamp01(distance / maxFadeDistance);

            Color c = sr.color;
            c.a = alpha;
            sr.color = c;
        }
    }
    public void Initialize(Transform spawner)
    {
        this.spawner = spawner;
    }
}
