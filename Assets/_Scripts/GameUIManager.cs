using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class GameUIManager : MonoBehaviour
{
    public static GameUIManager instance;

    public GameObject leftTurnButton;
    public GameObject rightTurnButton;
    public GameObject toggleRadarButton;
    public TMP_Text radarText;


    //public Volume postVolume;
    // Cache profile
    public VolumeProfile postProfile;


    public Radar radar;

    public void Awake()
    {
        instance = this;
    }

    public void Start()
    {
        // Load the post processing profile
        //postProfile = postVolume.profile);
        SetFocus(25);
    }

    public void Update()
    {
        leftTurnButton.SetActive(!radar.radarUp);
        rightTurnButton.SetActive(!radar.radarUp);

        radarText.text = radar.radarUp ? "<" : ">";
    }

    public void SetFocus(float newFocus)
    {
        DepthOfField de = null;
        int index = -1;
        for (int i = 0; i < postProfile.components.Count; i++)
        {
            if (postProfile.components[i].GetType() == typeof(DepthOfField))
            {
                de = (DepthOfField)(postProfile.components[i]);
               // de.focusDistance = new MinFloatParameter(newFocus, 0, true);
                index = i;
            }
        }

        if (de != null)
        {
            //de.focusDistance = new MinFloatParameter(newFocus, 0, true);
            StartCoroutine(LerpFocus(de, newFocus, 0.2f, index));
        }
    }

    public IEnumerator LerpFocus(DepthOfField de, float newFocus, float time, int index)
    {
        float t = 0;
        float start = de.focusDistance.value;

        while (t <= time)
        {
            t += Time.deltaTime;

            de.focusDistance.value = Mathf.Lerp(start, newFocus, t/time);

            postProfile.components[index] = de;
            yield return null; 
        }
    }
}
