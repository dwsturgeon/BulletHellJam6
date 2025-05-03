using UnityEngine.UI;
using UnityEngine;


public class Projectile : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private int bulletDamage = 20;

    private GameObject[] enemies;
    private GameObject enemy;

    
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

    //kill on collision
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Destroy(this.gameObject);
        }
        
        //For enemies that get affected by projectile
        if (collision.gameObject.tag == "Enemy")
        {
            
            Slider healthBar = collision.gameObject.GetComponentInChildren<Slider>();

            if ((healthBar.value - bulletDamage) <=0 )
            {
                Destroy(collision.gameObject);
            }
            else
            {
                healthBar.value = healthBar.value - bulletDamage;
            }
            
        }

        Destroy(this.gameObject);
    }
}
