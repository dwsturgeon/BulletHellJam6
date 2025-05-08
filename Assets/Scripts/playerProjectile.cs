using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float damageAmount = 10f;
    [SerializeField] private float killTimer = 12f;
    [SerializeField] private ProjectileType type;
    private Animator anim;
    private AudioSource exploAudio;

    public enum ProjectileType
    {
        Basic,
        Explosive,
        Crescent,
        Laser
    }


    private void Start()
    {
        if (type != ProjectileType.Laser)
        {
            Destroy(this.gameObject, killTimer);
        }
        exploAudio = GetComponent<AudioSource>();
    }
    private void Update()
    {
        MoveProjectile();
    }
    private void MoveProjectile()
    {
        transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
    }

    public void UpdateMoveSpeed(float speed)
    {
        this.moveSpeed = speed;
    }

    public float Damage
    { 
        get => damageAmount;
        set { damageAmount = value; }
    }

    public ProjectileType ProjType
    {
        get => type;
        set => type = value;
    }

    public void Explode()
    {
        anim = this.gameObject.GetComponentInChildren<Animator>();
        if (anim)
        {
            moveSpeed = 0f;
            this.GetComponent<Collider2D>().enabled = false;
            anim.SetBool("bExplode", true);
            exploAudio.Play();
            DestroyExplosive();
        }
    }

    public void DestroyExplosive()
    {
        Destroy(this.gameObject, 2f);
    }
        
}
