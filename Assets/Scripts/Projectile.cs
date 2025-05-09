using System.Runtime.CompilerServices;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    [SerializeField] private float moveSpeed = 0f;
    [SerializeField] public float damageAmount = 10f;
 

    private float originalMoveSpeed;
    private void Start()
    {
        originalMoveSpeed = moveSpeed;
    }
    private void Update()
    { 
        MoveProjectile();
    }


    public float Damage
    {
        get => damageAmount;
        set { damageAmount = value; }
    }

    private void MoveProjectile()
    {
        transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
    }

    public void UpdateMoveSpeed(float speed)
    {
        this.moveSpeed = speed;
    }

    public void ResetValue()
    {
        moveSpeed = originalMoveSpeed;
    }

    public void SlowDown(float multiplier)
    {
        moveSpeed = Mathf.Clamp(moveSpeed - (Time.deltaTime * multiplier), 1f, originalMoveSpeed);
    }

    public void SuperSlow()
    {
        moveSpeed = Mathf.Clamp(moveSpeed / ((Time.deltaTime * moveSpeed) + 1), 0, originalMoveSpeed);
    }

    public void Stop()
    {
        moveSpeed = Mathf.Clamp(moveSpeed -= (Time.deltaTime * 100), 0.1f, moveSpeed);
    }

    public void SpeedUp(float multiplier)
    {
        moveSpeed = Mathf.Clamp(moveSpeed + (Time.deltaTime * multiplier), originalMoveSpeed, originalMoveSpeed * 4);
    }

    public void Redirect(float randAngle)
    {
        Vector3 direction = (PlayerController.instance.transform.position - transform.position).normalized;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        angle += Random.Range(-randAngle, randAngle);

        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "DeathBarrier")
        {
            Destroy(this.gameObject);
        }
    }



}
