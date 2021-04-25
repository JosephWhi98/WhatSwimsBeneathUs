using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement; 

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

    public CanvasGroup screenFader;


    public Radar radar;

    public bool caught;

    public GameObject realMeg;
    public GameObject fakeMeg;
    public CameraController camController;
    public SharkCage cage;

    public TMP_Text finalText;
    public CanvasGroup finalTextGroup;

    public AudioSource source;
    public AudioClip drownClip; 

    public void Awake()
    {
        instance = this;
    }

    public void Start()
    {
        // Load the post processing profile
        //postProfile = postVolume.profile);
        SetFocus(25);

        screenFader.alpha = 1;
        StartCoroutine(DelayedFade());
    }

    public IEnumerator DelayedFade()
    {
        yield return new WaitForSeconds(1f);
        FadeScreen(screenFader, 0, 2);
    }

    public void Update()
    {
        if (!caught)
        {
            leftTurnButton.SetActive(!radar.radarUp && !radar.menuActive);
            rightTurnButton.SetActive(!radar.radarUp && !radar.menuActive);
            toggleRadarButton.SetActive(!radar.menuActive);

            radarText.text = radar.radarUp ? "<" : ">";
        }
        else
        {
            leftTurnButton.SetActive(false);
            rightTurnButton.SetActive(false);
            toggleRadarButton.SetActive(false);
        }
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

    public void FadeScreen(CanvasGroup group, float alpha, float time)
    {
        StartCoroutine(LerpFade(group, alpha, time));
    }

    public IEnumerator LerpFade(CanvasGroup group, float target, float time)
    {
        float t = 0;
        float start = group.alpha;

        while (t <= time)
        {
            t += Time.deltaTime;

            group.alpha = Mathf.Lerp(start, target, t / time);
            yield return null;
        }
    }

    public void Caught()
    {
        if (radar.radarUp)
        {
            radar.ToggleRadar();
        }

        if (cage.descending)
        {
            cage.ToggleDescending();
        }


        caught = true; 
        StartCoroutine(CaughtRoutine());
    }

    public IEnumerator CaughtRoutine()
    {
        FadeScreen(screenFader, 1, 1);
        yield return new WaitForSeconds(1.5f);
        realMeg.SetActive(false);
        camController.targetRotation = camController.startRotation; 
        FadeScreen(screenFader, 0, 3);
        yield return new WaitForSeconds(0.5f);
        fakeMeg.SetActive(true);
        yield return new WaitForSeconds(3.5f);
        FadeScreen(screenFader, 1, 1);
        yield return new WaitForSeconds(1f);
        finalText.text = "Depth: " + cage.depthDisplay.text;
        FadeScreen(finalTextGroup, 1, 1);

        yield return new WaitForSeconds(3f);
        FadeScreen(finalTextGroup, 0, 1);
        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene(0);
    }


    public void OxygenDeath()
    {
        if (radar.radarUp)
        {
            radar.ToggleRadar();
        }

        if (cage.descending)
        {
            cage.ToggleDescending();
        }

        caught = true;
        StartCoroutine(OxygenDeathRoutine());
    }

    public IEnumerator OxygenDeathRoutine()
    {
        //Oxygen death sfx
        source.PlayOneShot(drownClip);

        for (int i = 0; i < 2; i++)
        {
            FadeScreen(screenFader, 1, 0.5f);
            yield return new WaitForSeconds(1f);
            FadeScreen(screenFader, 0, 0.5f);
            yield return new WaitForSeconds(1f);
        }


        FadeScreen(screenFader, 1, 1);
        realMeg.SetActive(false);

        yield return new WaitForSeconds(1f);
        finalText.text = "Depth: " + cage.depthDisplay.text;
        FadeScreen(finalTextGroup, 1, 1);

        yield return new WaitForSeconds(3f);
        FadeScreen(finalTextGroup, 0, 1);
        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene(0);
    }
}
