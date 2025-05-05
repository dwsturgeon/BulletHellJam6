using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header("Range")]
    [SerializeField] private float minX;
    [SerializeField] private float maxX;
    [SerializeField] private float minY;
    [SerializeField] private float maxY;
    [SerializeField] private float waitTime;
    private float elapsedTime = 0f;
    private bool isWaiting = false;

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
            if(elapsedTime >= waitTime)
            {
                elapsedTime = 0;
                targetPos = GetRandomPoint();
            }
        } 
        else isWaiting = false;
    }

    public bool GetWaitStatus()
    {
        return isWaiting;
    }


    private Vector3 GetRandomPoint()
    {
        float randomX = Random.Range(minX, maxX);
        float randomY = Random.Range(minY, maxY);
        return new Vector3(randomX, randomY, 0);      
    }
}
