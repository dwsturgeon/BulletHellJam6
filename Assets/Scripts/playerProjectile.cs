using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class PlayerProjectile : MonoBehaviour
{
    [SerializeField] private Animator projAnim;
    public float _speed;
    public float _bulletDamage = 20;
    private Vector2 direction;

    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;
    }

    private void Start()
    {

        Destroy(this.gameObject, 10f);
    }

    private void Update()
    {
        transform.position += (Vector3)(direction * _speed * Time.deltaTime);
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
