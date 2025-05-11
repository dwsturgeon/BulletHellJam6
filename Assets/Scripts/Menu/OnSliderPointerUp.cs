using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class SliderDrag : MonoBehaviour, IPointerUpHandler
{

    [SerializeField]GameObject SoundManager;

    public void OnPointerUp(PointerEventData eventData)
    {
        SoundManager.GetComponent<SoundManager>().SetMusicVolume();
        SoundManager.GetComponent<SoundManager>().SetSFXVolume();
    }
}