using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class MovableObject : MonoBehaviour
{
    #region Variables
    private bool interactable = true;

    //public Transform[] handHolds;
    private Vector3 offset;
    public GameObject displayText;
    [SerializeField]
    private MovableObjectTrigger targetTrigger;

    private Transform player;
    private float distance;
    private bool beingDragged;
    private bool triggerActivated;

    private Rigidbody rb;
    #endregion

    void Start()
    {
        displayText.SetActive(false);
        offset = new Vector3(0, .65f, 0);

        if (player == null)
        {
            try
            {
                player = GameObject.Find("Player").transform; //Attempt to initialise player
            }
            catch (MissingComponentException)
            {
                print("ERROR in MovableObject.cs on " + name + ": No object is assigned to the \"Player\" variable, and no object called \"Player\" exists in the scene.");
            }
        }

        if (rb == null)
        {
            try
            {
                rb = GetComponent<Rigidbody>();
            }
            catch (MissingComponentException)
            {
                print("ERROR in MovableObject.cs on " + name + ": The object does not have a Rigidbody component attached to it.");
            }
        }
    }

    void Update()
    {
        if (interactable)
        {
            distance = Vector3.Distance(transform.position, player.position);

            if (distance < 2.5f && !beingDragged) //Check distance and if this is being manipulated
            {
                displayText.SetActive(true);
                if (Input.GetKeyDown(KeyCode.E)) //Check for pickup command
                    Drag();
            }
            else if (beingDragged && Input.GetKeyDown(KeyCode.E)) //Check for drop command
                Drop();
            else if (displayText.activeSelf && distance >= 2.5f) //Check for out-of-range state
                displayText.SetActive(false);
        }
        else
            Destroy(this);
    }

    public void Drag()
    {
        //Physics
        rb.useGravity = false;
        rb.isKinematic = true;
        transform.rotation = player.rotation;
        transform.position = player.position + (player.forward * 1.25f) + offset;
        //print("This is a fix");
        transform.parent = player;

        //Display and state
        displayText.SetActive(false);
        beingDragged = true;
    }

    public void Drop()
    {
        //Physics
        rb.useGravity = true;
        rb.isKinematic = false;
        transform.parent = null;

        //Display and state
        displayText.SetActive(false);
        beingDragged = false;
    }

    void OnTriggerStay(Collider other)
    {
        print("Stayed in collider.");

        if (!beingDragged && !triggerActivated && other.gameObject == targetTrigger.gameObject)
        {
            triggerActivated = true;
            print("Activating path...");
            targetTrigger.Activate();
            interactable = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        print("Entered collider.");
    }
}
