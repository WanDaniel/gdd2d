using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Orb : MonoBehaviour
{
    private Transform player;
    private Rigidbody rb;
    private bool canBeCollected = false;
    public GameObject tipText;
    private PuzzleManager puzzleManager;

    void Awake()
    {
        tipText.SetActive(false);
        puzzleManager = FindObjectOfType<PuzzleManager>();

        if (player == null)
        {
            try
            {
                player = GameObject.Find("Player").transform; //Attempt to initialise player
            }
            catch (Exception)
            {
                print("ERROR in Orb.cs: No object called \"Player\" exists in the scene.");
            }
        }

        try
        {
            rb = GetComponent<Rigidbody>(); //Attempt to initialise rigidbody
        }
        catch (MissingComponentException)
        {
            print("ERROR in Orb.cs: No RigidBody component exists on the orb.  Please verify that the prefab and all in-game instances have Rigidbodies.");
        }
    }
    
    void Update()
    {
        if (tipText.activeSelf)
        {
            tipText.transform.position = transform.position + Vector3.up;
        }

        if (canBeCollected && Vector3.Distance(transform.position, player.position) < 5)
        {
            if (!tipText.activeSelf)
                tipText.SetActive(true);

            if (Input.GetKeyDown(KeyCode.E))
            {
                puzzleManager.CollectOrb();
                Destroy(gameObject);
            }
        }
        else if (tipText.activeSelf)
            tipText.SetActive(false);
    }

    public void AllowCollection()
    {
        canBeCollected = true;
        rb.useGravity = true;
    }
}
