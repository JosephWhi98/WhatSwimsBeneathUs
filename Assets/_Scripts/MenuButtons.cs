using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.Events; 

public class MenuButtons : MonoBehaviour
{
    [SerializeField] TMP_Text text;

    bool hovering;

    public UnityEvent clickEvent;
    public AudioSource audioSource;
    public AudioClip hoverClip;
    public AudioClip clickClip;


    public void OnClick()
    {
        clickEvent.Invoke();
        audioSource.clip = clickClip;
        audioSource.Play();
    }


    public void MouseEnter()
    {
        hovering = true;
        transform.localScale = Vector3.one * 0.11f;
        audioSource.clip = hoverClip;
        audioSource.Play();
    }


    public void MouseExit()
    {
        hovering = false;
        transform.localScale = Vector3.one * 0.1f;
    }
}
