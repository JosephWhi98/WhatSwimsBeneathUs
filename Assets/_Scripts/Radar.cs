using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radar : MonoBehaviour
{
    public Transform rotator;
    public Transform player;

    public float maxDist;
    public Transform trackingTarget;
    public GameObject radarIcon;

    private float nextPlayTime; 
    public AudioSource audioSource;
    public AudioClip radarPing;

    public bool radarUp;
    public Animator animator;

    public void Update()
    {
        rotator.Rotate(new Vector3(0, 0, -1f), Space.Self);

        Vector3 trackingPosition = trackingTarget.position;
        trackingPosition.y = player.position.y + 5f; 
            
        if (Vector3.Distance(trackingTarget.position, player.position) < maxDist)
        {
            radarIcon.SetActive(true);
            float p = Mathf.PingPong(Time.time * 1f, 1);

            if (p > .9f && Time.time > nextPlayTime && radarUp)
            {
                nextPlayTime = Time.time + 0.5f;
                audioSource.PlayOneShot(radarPing);
            }

            radarIcon.transform.localScale = (Vector3.one * 0.5f) * p;
            radarIcon.transform.localPosition = player.InverseTransformPoint(trackingPosition) / 12f;
        }
        else
        {
            radarIcon.SetActive(false);
        }
    }

    public void ToggleRadar()
    {
        radarUp = !radarUp;
        animator.SetBool("Equiped", radarUp);

        float focus = radarUp ? 0.22f : 25f;
        GameUIManager.instance.SetFocus(focus);
    }
}
