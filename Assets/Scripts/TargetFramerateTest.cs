using UnityEngine;

public class TargetFramerateTest : MonoBehaviour
{
    [SerializeField] private int targetFramerate = 120;
    private void FixedUpdate()
    {
        if (Application.targetFrameRate != targetFramerate) Application.targetFrameRate = targetFramerate;
    }
    private void Start()
    {
        Application.targetFrameRate = 360;
        Screen.SetResolution(1920, 1080, true);
    }
}
