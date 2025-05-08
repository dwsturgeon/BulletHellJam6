using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{

    private static SoundManager instance;
    [SerializeField] AudioMixer audioMixer;

    [SerializeField] Slider musicSlider;
    [SerializeField] Slider sfxSlider;

    public float masterVolume = 1.0f; // Initial value

    void Awake()
    {
        instance = this;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
