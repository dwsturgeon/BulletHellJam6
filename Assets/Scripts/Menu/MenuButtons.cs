using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuButtons : MonoBehaviour
{

    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip hoverClip;
    [SerializeField] AudioClip pressClip;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        audioSource.clip = hoverClip;
        audioSource.Play();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        audioSource.clip = pressClip;
        audioSource.Play();
    }

}
