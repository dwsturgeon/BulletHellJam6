using System.Collections;
using UnityEngine;

public class PlaceHolderEmemyMovement : MonoBehaviour
{
    private Vector3 firstPoint = new Vector3(7f, 4.23f, 0);
    private Vector3 secondPoint = new Vector3(-7f, 4.23f, 0);
    private Vector3 targetPostion;
    private float smoothTime = 600f;
    private float threshhold = 1f;

    private Vector3 velocity = Vector3.zero;

    private void Start()
    {
        targetPostion = firstPoint;
    }
    void Update()
    {
        transform.position = Vector3.SmoothDamp(transform.position, targetPostion, ref velocity, smoothTime * Time.deltaTime);

        if(Vector3.Distance(transform.position, targetPostion) < threshhold)
        {
            targetPostion = targetPostion == firstPoint ? secondPoint : firstPoint;
        }
    }

    
}
