using UnityEngine;

public class MusicLooper : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip introClip;
    [SerializeField] private AudioClip loopClip;
    [SerializeField] private AudioClip endIntroClip;
    [SerializeField] private AudioClip endLoopClip;

    public bool isDead = false;
    private bool hasStartedLoop = false;
    private float elapsed;

    private void Start()
    {
        audioSource.clip = introClip;
        audioSource.Play();

    }

    private void Update()
    {
        elapsed += Time.deltaTime;
        if (elapsed > 1f)
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

    public void PlayEndMusic()
    {
        audioSource.Stop();
        hasStartedLoop = false;
        audioSource.clip = endIntroClip;
        audioSource.Play();
        loopClip = endLoopClip;
        elapsed = 0;
    }
}

