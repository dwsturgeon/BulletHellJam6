using Unity.VisualScripting;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header("Range")]
    [SerializeField] private float minX;
    [SerializeField] private float maxX;
    [SerializeField] private float minY;
    [SerializeField] private float maxY;
    [SerializeField] private float waitTime;
    [SerializeField] private float minMoveDistance;
    private float elapsedTime = 0f;
    private bool isWaiting = false;
    private bool gotHit = false;

    [Header("Smooth Settings")]
    [SerializeField] private float smoothTime = 1f;
    [SerializeField] private float threshhold = 1f;



    private Vector3 velocity = Vector3.zero;

    private Vector2 targetPos;

    private void Start()
    {
        targetPos = GetRandomPoint();
    }
    private void Update()
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

    public bool GetWaitStatus()
    {
        return isWaiting;
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        gotHit = true;
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
