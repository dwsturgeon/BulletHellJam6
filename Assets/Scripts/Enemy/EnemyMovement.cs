using Unity.VisualScripting;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Range")]
    [SerializeField] private float minX;
    [SerializeField] private float maxX;
    [SerializeField] private float minY;
    [SerializeField] private float maxY;
    
    private float elapsedTime = 0f;
    private bool isWaiting = false;
    private bool gotHit = false;
    

    [Header("Movement Settings")]
    [SerializeField] private float smoothTime = 1f;
    [SerializeField] private float threshhold = 1f;
    [SerializeField] private float waitTime;
    [SerializeField] private float minMoveDistance;

    [Header("Stun Settings")]
    [SerializeField] private float recoveryMult = 1f;
    private bool stunned = false;
    private float stunTimer = 1f;

    [Header("Animator")]
    [SerializeField] Animator animator;

    private Vector3 velocity = Vector3.zero;

    private Vector2 targetPos;

    private void Start()
    {
        targetPos = GetRandomPoint();
    }
    private void Update()
    {
        if (stunned)
        {
            animator.SetBool("isStunned", true);
            if (stunTimer >= 0f)
            {
                stunTimer -= (recoveryMult * Time.deltaTime);
            }
            else
            {
                stunned = false;
                animator.SetBool("isStunned", false);
            }


        }
        else
        {
            transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, smoothTime);

            if(Vector3.Distance(transform.position, targetPos) < threshhold)
            {
                elapsedTime += Time.deltaTime;
                isWaiting = true;
                if (elapsedTime >= waitTime && !gotHit)
                {
                    elapsedTime = 0;
                    targetPos = GetRandomPoint();
                }
                else if(elapsedTime >= waitTime && gotHit)
                {
                    OnHit(minMoveDistance);
                }
            } 
            else isWaiting = false;
        }


    }

    public bool GetWaitStatus()
    {
        return isWaiting;
    }

    public void SetStunned(float _stunT)
    {
        stunTimer = _stunT;
        stunned = true;
    }

    private void OnHit(float minDistance)
    {
        const int maxAttempts = 10;
        int attempts = 0;
        Vector2 newTarget = targetPos;

        while (attempts < maxAttempts)
        {
            float randomX = Random.Range(minX, maxX);
            float randomY = Random.Range(minY, maxY);

            Vector2 candidate = new Vector2(randomX, randomY);
            if (Vector2.Distance(transform.position, candidate) >= minDistance)
            { 
                newTarget = candidate;
                break;
            }
            attempts++;
        }

        targetPos = new Vector3(Mathf.Clamp(newTarget.x, minX, maxX), Mathf.Clamp(newTarget.y, minY, maxY), 0f);
        gotHit = false;
        elapsedTime = 0;                             
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        gotHit = true;

        if(collision.GetComponent<PlayerProjectile>().ProjType == PlayerProjectile.ProjectileType.Explosive)
        {
            PlayerProjectile projectile = collision.GetComponent<PlayerProjectile>();
            projectile.Explode();
        }

    }

    private Vector3 GetRandomPoint()
    {
        float randomX = Random.Range(minX, maxX);
        float randomY = Random.Range(minY, maxY);
        return new Vector3(randomX, randomY, 0);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawCube(new Vector3((minX + maxX) / 2f, (minY + maxY) / 2f, 0f), new Vector3(maxX - minX, maxY - minY, 0));
    }
}
