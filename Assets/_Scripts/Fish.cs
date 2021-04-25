using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : MonoBehaviour
{
    public Transform[] target;
    protected Vector3 newTarget;

    protected bool isMoving = false;

    public float speed = 2.0f;
    public float animationSpeed = 2.0f;

    float randomOffset;

    public Transform[] bones;

    public Transform cagePosition;
    public float rangeFromPlayer;

    public float rotationSpeed = 1f;
    protected Quaternion targetRotation;

    Collider cageCollider;

    public LayerMask avoidanceMask; 

    private void Start()
    {
        randomOffset = Random.Range(0, 1000);
        cageCollider = cagePosition.transform.GetComponent<Collider>();
    }

    public virtual void Update()
    {
        if (isMoving == false)
        {
            UpdateTarget();
            isMoving = true;
        }

        if (Vector3.Distance(transform.position, newTarget) < 3f)
        {
            isMoving = false;
        }

        Vector3 targetDirection = (newTarget - transform.position).normalized;

        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 7, avoidanceMask))
        {
            targetDirection += hit.normal * 20;
        }

        targetRotation = Quaternion.LookRotation(targetDirection);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);

        transform.position = Vector3.MoveTowards(transform.position, transform.position + transform.forward, speed * Time.deltaTime);


        UpdateBones();
    }

    public void UpdateBones()
    {
        int i = 1;
        foreach (Transform b in bones)
        {
            Vector3 euler = b.localEulerAngles;

            euler.z = Mathf.Sin((Time.time + randomOffset) * 1.5f * animationSpeed * i) * 10;

            b.localEulerAngles = euler;
            i++;
        }
    }

    public virtual void UpdateTarget()
    {
        newTarget = cagePosition.position + (Random.onUnitSphere * rangeFromPlayer);
    }

    IEnumerator SpeedBoostRoutine()
    {
        yield return new WaitForSeconds(2f);
        speed /= 4;
    }

 
    private void OnDestroy()
    {
        StopAllCoroutines();
    }

}
