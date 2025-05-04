using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1f;

    private void Start()
    {
        Destroy(this.gameObject, 5f);
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
       GameObject enemy = collision.gameObject;
       if(enemy.tag == "Enemy")
        {
            //do damage
        }

       //explode the projectile

       Destroy(this.gameObject);
    }

}
