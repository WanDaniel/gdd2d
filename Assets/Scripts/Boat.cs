using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boat : MonoBehaviour
{
    public float rotationSpeed;
    private Quaternion rotationA, rotationB;
    private bool rotation;

    void Awake()
    {
        if (rotationSpeed <= 0)
            rotationSpeed = 1.5f;

        rotationA = Quaternion.Euler(-3, -40, -2);
        rotationB = Quaternion.Euler(2.5f, -40, 1.25f);
    }

    void Update()
    {
        Rock();
    }

    void Rock()
    {
        if (rotation)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotationA, rotationSpeed * Time.deltaTime);

            if (Quaternion.Angle(transform.rotation, rotationA) <= 1f)
            {
                rotation = false;
            }
        }
        else
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotationB, rotationSpeed * Time.deltaTime);

            if (Quaternion.Angle(transform.rotation, rotationB) <= 1f)
            {
                rotation = true;
            }
        }
    }
}
