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

    public Vector3 startScale;

    public void Start()
    {
        startScale = transform.localScale;
    }


    public void OnClick()
    {
        clickEvent.Invoke();
        audioSource.clip = clickClip;
        audioSource.Play();
    }


    public void MouseEnter()
    {
        hovering = true;
        transform.localScale = startScale * 1.1f;
        audioSource.clip = hoverClip;
        audioSource.Play();
    }


    public void MouseExit()
    {
        hovering = false;
        transform.localScale = startScale;
    }
}
