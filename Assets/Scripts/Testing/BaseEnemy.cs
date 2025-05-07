using UnityEngine;

public class BaseEnemy : MonoBehaviour
{

    private void Start()
    {
        print("started from BaseEnemy");
    }
    void SayHello()
    {
        print("allo from BaseEnemy");
    }

    private void FixedUpdate()
    {
        SayHello();
    }

    private void testFunc()
    {
        print("sigma");
    }

}
