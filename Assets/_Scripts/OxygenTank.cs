using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OxygenTank : MonoBehaviour
{
    public float oxygen;
    private float oxygenMax;

    public Transform dial;


    public void Start()
    {
        oxygenMax = oxygen;
    }


    public void Update()
    {
        Vector3 rot = dial.transform.localEulerAngles;

        rot.z = Mathf.Lerp(180, 15, 1 - oxygen/oxygenMax);
        dial.transform.localEulerAngles = rot;


        oxygen -= Time.deltaTime;
    }
}
