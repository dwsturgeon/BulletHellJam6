using UnityEngine;

public class BulletContainer : MonoBehaviour
{
    public Transform spawner;
    void Start()
    {
        foreach (FadeByDistance proj in GetComponentsInChildren<FadeByDistance>())
        {
            proj.Initialize(spawner);
        }
        Destroy(this.gameObject, 10);
        Destroy(this);
    }
}
