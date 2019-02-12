using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour {

    public TriggerObject[] objectsToTrigger;
    private Vector3 upPos;
    private Vector3 downPos;

    void Awake()
    {
        upPos = transform.position;
        downPos = new Vector3(transform.position.x, transform.position.y - .025f, transform.position.z);
    }

    public void Activate()
    {
        for (int i = 0; i < objectsToTrigger.Length; i++)
            objectsToTrigger[i].Trigger();

        transform.position = downPos;
    }

    public void ResetPlate()
    {
        transform.position = upPos;
    }
}
