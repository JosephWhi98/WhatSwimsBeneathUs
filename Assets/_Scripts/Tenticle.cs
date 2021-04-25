using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tenticle : MonoBehaviour
{
    public Transform[] bones;
    public float speed = 2.0f;
    public float animationSpeed = 2.0f;

    float randomOffset;

    private void Start()
    {
        randomOffset = Random.Range(0, 999f);
    }

    void Update()
    {

        int i = 1;
        foreach (Transform b in bones)
        {
            Vector3 euler = b.localEulerAngles;

            euler.z = Mathf.Sin((Time.time + randomOffset) * 1.5f * animationSpeed * i) * 10;
            euler.x = Mathf.Cos((Time.time + randomOffset) * 1.5f * animationSpeed * i) * 10;

            b.localEulerAngles = euler;
            i++;
        }

    }

}
