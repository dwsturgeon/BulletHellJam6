using UnityEngine;

public class TargetFramerateTest : MonoBehaviour
{
    [SerializeField] private int targetFramerate = 120;

    private void Update()
    {
        if (Application.targetFrameRate != targetFramerate) Application.targetFrameRate = targetFramerate;
    }
}
