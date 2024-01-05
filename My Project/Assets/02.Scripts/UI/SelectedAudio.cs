using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectedAudio : MonoBehaviour, IPointerEnterHandler
{
    private new AudioSource audio;

    public AudioClip selectedAudio;
    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponent<AudioSource>();
        if(audio == null)
        {
            audio = gameObject.AddComponent<AudioSource>();
;        }
        audio.clip = selectedAudio;
        audio.playOnAwake = false;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        audio.Play();
    }
}
