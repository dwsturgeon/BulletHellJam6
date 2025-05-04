using UnityEngine;

public class GlitchEffect : MonoBehaviour
{
    [SerializeField] private Material defaultMaterial;
    [SerializeField] private GameObject visualTarget;
    [SerializeField] private Material glitchMaterial;
    private Collider2D playerCollider;

    private SpriteRenderer sr;
    private bool isGlitching = false;

    private void Start()
    {
        sr = visualTarget.GetComponent<SpriteRenderer>();
        sr.material = defaultMaterial;
        playerCollider = GetComponent<Collider2D>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.LeftControl))
        {
            toggleGlitch();
        }
    }

    void toggleGlitch()
    {
        isGlitching = !isGlitching;
        sr.material = isGlitching ? glitchMaterial : defaultMaterial;
                        
        if (isGlitching) playerCollider.enabled = false;
        else playerCollider.enabled = true;

    }
}
