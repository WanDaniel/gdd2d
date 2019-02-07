using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.AI;

public class Follower : MonoBehaviour
{
    #region Variables
    //Player tracking
    /// <summary>
    /// The object this character should follow.
    /// </summary>
    [SerializeField]
    Transform player;
    /// <summary>
    /// The NavMeshAgent component controlling this character.
    /// </summary>
    NavMeshAgent agent;
    [SerializeField]
    /// <summary>
    /// The current distance between this character and the Transform variable "player."
    /// </summary>
    float distanceToPlayer;
    /// <summary>
    /// The minimum distance this character will maintain from the Transform variable "player."
    /// </summary>
    public float followDistance;
    /// <summary>
    /// The position this character is current moving towards.
    /// </summary>
    Transform target;
    #endregion

    void Awake()
    {
        try
        {
            agent = GetComponent<NavMeshAgent>(); //Attempt to initialise NavMeshAgent
        }
        catch (MissingComponentException)
        {
            print("ERROR in Follower.cs: The object \"" + name + "\" does not have a NavMeshAgent component attached to it.");
        }

        if (player == null)
        {
            try
            {
                player = GameObject.Find("Player").transform; //Attempt to initialise player
            }
            catch (MissingComponentException)
            {
                print("ERROR in Follower.cs: No object is assigned to the \"Player\" variable, and no object called \"Player\" exists in the scene.");
            }
        }
    }

    void Update()
    {
        distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer >= followDistance)
            target = player;
        else
            target = transform;

        agent.SetDestination(target.position);
    }
}
