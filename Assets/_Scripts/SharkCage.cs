using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SharkCage : MonoBehaviour
{
    public bool descending;
    public Light warningLight;
    public CameraShake shake;

    public float descendingSpeed;

    public AudioSource audioSource;
    public AudioClip loweringClip;

    public TMP_Text depthDisplay;
    public float startingDepth = 300;

    public Meg meg;

    public float heat; 

    public void Update()
    {
        float depth = startingDepth + Mathf.Abs(transform.position.y);
        depthDisplay.text = depth.ToString("n0") + "m";

        shake.shaking = descending;
        warningLight.enabled = descending;

        if (descending)
        {
            warningLight.intensity = Mathf.PingPong(Time.time * 15f, 10f);

            transform.localPosition = Vector3.Lerp(transform.localPosition, transform.localPosition - transform.up, Time.deltaTime * descendingSpeed);
            audioSource.volume = Mathf.Lerp(audioSource.volume, 1f, Time.deltaTime * 3f);


            float heatMult = 1;

            if (Mathf.Abs(transform.position.y) > 100)
                heatMult = 2f;
            else if (Mathf.Abs(transform.position.y) > 150)
                heatMult = 3f;
            else if (Mathf.Abs(transform.position.y) > 200)
                heatMult = 4f;
            else if (Mathf.Abs(transform.position.y) > 250)
                heatMult = 5f;

            heat += (Time.deltaTime * heatMult);
        }
        else
        {
            audioSource.volume = Mathf.Lerp(audioSource.volume, 0f, Time.deltaTime * 3f);
            heat -= Time.deltaTime;
        }

        heat = heat < 0 ? 0 : heat; //clamp heat at 0

        
        Debug.Log(heat);

    }


    public void ToggleDescending()
    {
        if (GameUIManager.instance.caught == false)
        {
            descending = !descending;
            meg.UpdateTarget();
        }
    }

}
