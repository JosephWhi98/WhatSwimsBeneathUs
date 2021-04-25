using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OxygenTank : MonoBehaviour
{
    public float oxygen;
    private float oxygenMax;

    public Transform dial;


    public AudioSource source;

    public void Start()
    {
        oxygenMax = oxygen;
    }


    public void Update()
    {
        Vector3 rot = dial.transform.localEulerAngles;

        rot.z = Mathf.Lerp(180, 15, 1 - oxygen/oxygenMax);
        dial.transform.localEulerAngles = rot;

        if (oxygen / oxygenMax < 0.1f)
        {
            if (!source.isPlaying)
            {
                source.volume = 0;
                source.Play();
            }
            else
            {
                source.volume = Mathf.Lerp(source.volume, 0.413f, Time.deltaTime * 5f);
            }
        }

        if (!GameUIManager.instance.radar.menuActive && !GameUIManager.instance.caught)
        {
            oxygen -= Time.deltaTime;

            if (oxygen <= 0)
            {
                oxygen = 0;
                GameUIManager.instance.OxygenDeath();
            }
        }
    }
}
