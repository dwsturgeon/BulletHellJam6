using System.Collections;
using UnityEngine;

public class PlaceHolderEmemyMovement : MonoBehaviour
{
    private Vector3 firstPoint = new(7f, 4f, 0);
    private Vector3 secondPoint = new(-7f, 4f, 0);
    private Vector3 targetPostion;
    [SerializeField] private float smoothTime = 1f;
    private float threshhold = 1f;

    private Vector3 velocity = Vector3.zero;

    private void Start()
    {
        targetPostion = firstPoint;
    }
    void Update()
    {
        transform.position = Vector3.SmoothDamp(transform.position, targetPostion, ref velocity, smoothTime);

        if(Vector3.Distance(transform.position, targetPostion) < threshhold)
        {
            targetPostion = targetPostion == firstPoint ? secondPoint : firstPoint;
        }
    }

    
}
