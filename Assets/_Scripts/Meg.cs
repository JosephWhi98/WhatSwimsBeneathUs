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


        float dangerDist = 5f;
        float maxHeat = 35f;

        if (Mathf.Abs(cagePosition.position.y) > 100)
        {
            dangerDist = 6f;
            maxHeat = 30f;
        }
        else if (Mathf.Abs(cagePosition.position.y) > 150)
        {
            dangerDist = 7f;
            maxHeat = 25f;
        }
        else if (Mathf.Abs(cagePosition.position.y) > 200)
        {
            dangerDist = 8f;
            maxHeat = 20f;
        }
        else if (Mathf.Abs(cagePosition.position.y) > 250)
        {
            dangerDist = 9f;
            maxHeat = 15f;
        }




        if (Vector3.Distance(transform.position, cagePosition.position) < dangerDist && cage.descending || cage.heat > maxHeat)
        {
            if (GameUIManager.instance.caught == false)
                GameUIManager.instance.Caught();
        }
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
