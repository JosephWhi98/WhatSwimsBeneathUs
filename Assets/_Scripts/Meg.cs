using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meg : Fish
{
    public SharkCage cage;
    public Radar radar; 
    public float rangeWhenCageIsFalling;
    public float exploreRange;

    public float normalSpeed;
    public float sprintSpeed;

    public override void Update()
    {

        float expectedRange = cage.descending || cage.heat > 5f ? rangeWhenCageIsFalling : exploreRange;
        bool forceUpdateTarget = cage.descending && (Vector3.Distance(cagePosition.position, transform.position) > 20);

        if (!radar.menuActive)
        {
            bool sprint = cage.descending && cage.heat > 5f && (Vector3.Distance(cagePosition.position, transform.position) > 30);
            speed = sprint ? sprintSpeed : normalSpeed;
        }
        else
        {
            speed = 0;
        }

        if (isMoving == false || rangeFromPlayer != expectedRange || Vector3.Distance(transform.position, newTarget) < 1f || forceUpdateTarget)
        {
            isMoving = true;
            rangeFromPlayer = expectedRange;
            UpdateTarget();
        }

        transform.position = Vector3.MoveTowards(transform.position, transform.position + transform.forward, speed * Time.deltaTime);

        Vector3 targetDirection = (newTarget - transform.position).normalized;

        targetRotation = Quaternion.LookRotation(targetDirection);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);

        UpdateBones();
    }

    public override void UpdateTarget()
    {
        bool sprint = cage.descending && (Vector3.Distance(cagePosition.position, transform.position) > 20);

        if (!sprint)
            newTarget = cagePosition.position + (Random.onUnitSphere * rangeFromPlayer);
        else
            newTarget = cagePosition.position + Vector3.left * 3f; 
    }
}
