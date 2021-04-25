using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioEvent : MonoBehaviour
{
    public AudioClip clip;
    public AudioSource source;


    public void PlayOne()
    {
        source.pitch = Random.Range(0.95f, 1.05f);
        source.PlayOneShot(clip);
    }
}
 