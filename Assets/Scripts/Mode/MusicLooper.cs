using UnityEngine;

public class MusicLooper : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip introClip;
    [SerializeField] private AudioClip loopClip;


    private bool hasStartedLoop = false;

    private void Start()
    {
        audioSource.clip = introClip;
        audioSource.Play();

    }

    private void Update()
    {
        if (!audioSource.isPlaying && !hasStartedLoop)
        {
            hasStartedLoop = true;
            audioSource.clip = loopClip;
            audioSource.loop = true;
            audioSource.Play();
        }
    }
}

