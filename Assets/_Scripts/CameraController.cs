using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float rotationSpeed; 
    public Quaternion targetRotation;
    public Quaternion startRotation;
    public AudioSource audioSource;
    public Transform megladon;


    public void Start() 
    {
        startRotation = transform.localRotation; 
        targetRotation = startRotation; 
    }

    public void Update()
    {
        float dist = Vector3.Distance(megladon.position, transform.position);
        if (dist < 50f)
        {
          audioSource.volume = Mathf.Lerp(audioSource.volume, 1 - dist/50f, Time.deltaTime * 3f);
        }
        else
        {
            audioSource.volume = Mathf.Lerp(audioSource.volume, 0f, Time.deltaTime * 3f);
        }

        transform.localRotation = Quaternion.Lerp(transform.localRotation, targetRotation, Time.deltaTime * rotationSpeed);
    }

    public void Rotate(float ammount)
    {
        targetRotation = targetRotation * Quaternion.AngleAxis(ammount, Vector3.up);
    }

}
