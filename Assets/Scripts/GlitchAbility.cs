using UnityEngine;

public class GlitchAbility : MonoBehaviour
{
    [Header("Mat Settings")]
    [SerializeField] private Material defaultMaterial;
    [SerializeField] private GameObject visualTarget;
    [SerializeField] private Material glitchMaterial;

    [Header("Ability Settings")]
    [SerializeField] private float glitchDuration = 1f;
    [SerializeField] private float cooldownTime = 5f;
    [SerializeField] private float dodgeForce = 10f;

    [Header("Sound")]
    [SerializeField] AudioSource AudioSource;


    private float cooldownTimer = 0f;
    private float glitchTimer = 0f;
    private bool isGlitching = false;

    private Rigidbody2D playerRB;
    private Collider2D playerCollider;

    private SpriteRenderer sr;
    

    private void Start()
    {
        sr = visualTarget.GetComponent<SpriteRenderer>();
        sr.material = defaultMaterial;
        playerCollider = GetComponent<Collider2D>();
        playerRB = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.LeftControl) && !isGlitching && cooldownTimer <= 0f)
        {
            ActivateGlitch();
        }

        if (isGlitching) 
        {
            glitchTimer += Time.deltaTime;
      
            if (glitchTimer >= glitchDuration)
            {
                EndGlitch();
            }

        }

        if (cooldownTimer > 0f)
        {
            cooldownTimer -= Time.deltaTime;
        }



    }

    void ActivateGlitch()
    {
        PlayGlitchSound();
        isGlitching = true;
        glitchTimer = 0f;
        cooldownTimer = cooldownTime;
        sr.material = glitchMaterial;
        SetGlitchingStatus(true);
        PushPlayer();
    }

    void EndGlitch()
    {
        isGlitching = false;
        sr.material = defaultMaterial;
        SetGlitchingStatus(false);
    }

    private void SetGlitchingStatus(bool status)
    {
        this.gameObject.GetComponent<Health>().isGlitching = status;
        this.gameObject.GetComponent<FlashOnCollision>().isGlitching = status;
        this.gameObject.GetComponent<ShakeOnCollision>().isGlitching = status;
    }

    private void PlayGlitchSound()
    {
        AudioSource.pitch = Random.Range(1f, 1.2f);
        AudioSource.Play();
    }
    private void PushPlayer()
    {
        Vector2 moveDirection;
        if (playerRB.linearVelocity != Vector2.zero)
        {
            moveDirection = playerRB.linearVelocity.normalized;
            playerRB.AddForce(moveDirection * dodgeForce, ForceMode2D.Impulse);
        }
    }
}
