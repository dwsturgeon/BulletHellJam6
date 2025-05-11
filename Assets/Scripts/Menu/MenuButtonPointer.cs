using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuButtons : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler
{

    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip hoverClip;
    [SerializeField] AudioClip pressClip;

    public void OnPointerEnter(PointerEventData eventData)
    {
        audioSource.clip = hoverClip;
        audioSource.Play();
        Debug.Log("Pointer Enter");
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        audioSource.clip = pressClip;
        audioSource.Play();
        Debug.Log("Pointer Enter");
    }

}
