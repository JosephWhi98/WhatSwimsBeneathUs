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

    public bool menuActive;

    public GameObject radarBase;
    public GameObject menuBase;
    public GameObject radarDead;

    public GameObject[] batteryBars;

    public float charge;
    private float startCharge;

    public GameObject mainMenu;
    public GameObject helpMenu;

    public void Start()
    {
        menuActive = true;
        radarUp = false;
        startCharge = charge;

        StartCoroutine(DelayedRaise());
    }

    IEnumerator DelayedRaise()
    {
        yield return new WaitForSeconds(1.5f);
        ToggleRadar();
    }



    public void Update()
    {
        radarBase.SetActive(!menuActive && charge > 0);
        radarDead.SetActive(!menuActive && charge <= 0);

        menuBase.SetActive(menuActive);

        if (!menuActive)
        { 
            if(charge > 0)
                RadarUpdate();

            if (radarUp)
            {
                float batPerc = charge / startCharge;

                batteryBars[0].SetActive(batPerc > 0.75f);
                batteryBars[1].SetActive(batPerc > 0.50f);
                batteryBars[2].SetActive(batPerc > 0.25f);
                batteryBars[3].SetActive(batPerc > 0f);

                charge = charge <= 0 ? 0 : charge - Time.deltaTime;
            }
        }

    }

    public void RadarUpdate()
    {
        rotator.Rotate(new Vector3(0, 0, -200f * Time.deltaTime), Space.Self);

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

    public void ToggleMenu()
    {
        ToggleRadar();
        StartCoroutine(DelayedMenuDisable());
    }

    public IEnumerator DelayedMenuDisable()
    {
        yield return new WaitForSeconds(1f);
        menuActive = false;
    }

    public void HowTo()
    {
        mainMenu.SetActive(false);
        helpMenu.SetActive(true);
    }

    public void BackToMain()
    {
        mainMenu.SetActive(true);
        helpMenu.SetActive(false);
    }

    public void Exit()
    {
#if !UNITY_EDITOR
    Application.Quit();
#endif
    }
}

