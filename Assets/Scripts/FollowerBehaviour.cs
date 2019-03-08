using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FollowerBehaviour : MonoBehaviour {

    #region Variables
    AudioManager audioManager;

    private bool helping = false;
    private Orb orbHelpTarget;

    private Rigidbody rigidbody;

    #region Floating
    /// <summary>
    /// The "energy" emenating from the follower.
    /// </summary>
    [SerializeField]
    ParticleSystem particleSys;
    /// <summary>
    /// The "energy" emenating from the follower.  This is the mainModule of the variable 'particleSys.'
    /// </summary>
    ParticleSystem.MainModule particles;
    /// <summary>
    /// The default position for the "obj" GameObject variable.
    /// </summary>
    [SerializeField]
    Transform defaultPos;
    /// <summary>
    /// Position toward which the object is floating.
    /// </summary>
    [SerializeField]
    Vector3 floatTarget;
    /// <summary>
    /// Used to control smooth elevation control of "obj" GameObject variable.
    /// </summary>
    Vector3 velocity = Vector3.zero;
    /// <summary>
    /// Speed at which the character floats/hovers.  Adjust based on frustration.
    /// </summary>
    [SerializeField]
    float floatSpeed;
    /// <summary>
    /// Distance and sporadicalness of the character's floating.  Adjust based on frustration.
    /// </summary>
    [SerializeField]
    float floatDistance;
    #endregion

    #region Frustration
    /// <summary>
    /// The integer value of the character's current frustration.  Used to quantify reactions.
    /// </summary>
    [SerializeField]
    int frustrationStage = 0;
    /// <summary>
    /// The amount of time between frustration stage increases.  Default (max) value for float "frustrationTimer."
    /// </summary>
    public float frustrationIntervals;
    /// <summary>
    /// Amount of time remaining before frustration increase.
    /// </summary>
    [SerializeField]
    float frustrationTimer;
    /// <summary>
    /// Is the character currently growing frustrated?
    /// </summary>
    bool furious;
    /// <summary>
    /// Material used during frustration fluctuation.
    /// </summary>
    [SerializeField]
    Material[] materials;
    /// <summary>
    /// Used in HitThings() to determine when the character should ascend or descend.
    /// </summary>
    bool hitGround;
    /// <summary>
    /// The top point the character will move to when furious.
    /// </summary>
    [SerializeField]
    Transform topPos;
    #endregion
    #endregion

    void Awake()
    {
        audioManager = AudioManager.instance;

        if (defaultPos == null)
            print("ERROR in Follower.cs: The Transform \"Default Pos\" is unassigned.  This should be a child object of the follower.");
        else
        {
            //Generate initial float target if floating is possible
            floatTarget = new Vector3(UnityEngine.Random.Range(-floatDistance, floatDistance), UnityEngine.Random.Range(-floatDistance, floatDistance), UnityEngine.Random.Range(-floatDistance, floatDistance)) + defaultPos.position;
        }

        try
        {
            rigidbody = GetComponent<Rigidbody>(); //Attempt to initialise rigidbody
        }
        catch (MissingComponentException)
        {
            print("ERROR in FolowerBehaviour.cs: No RigidBody component exists on the follower object.  Please verify that the prefab and in-game instance have Rigidbodies.");
        }

        //Initialisation if given values are unusable
        if (floatSpeed == 0)
            floatSpeed = 3;
        if (floatDistance == 0)
            floatDistance = .25f;
        if (frustrationIntervals == 0)
            frustrationIntervals = 5;
        frustrationTimer = frustrationIntervals;

        GetComponent<MeshRenderer>().material = materials[0];
        particles = particleSys.main;
        particles.startColor = materials[0].color;
    }

    void Update()
    {
        if (!helping)
        {
            CheckFrustrate();

            if (!furious)
                Float();
        }

        if (helping && orbHelpTarget)
            KnockOrb();
    }

    void Float()
    {
        if (Input.anyKey)
        {
            floatTarget = defaultPos.position;
        }
        else
        {
            //Generate a random position nearby
            floatTarget = new Vector3(
                UnityEngine.Random.Range(-floatDistance, floatDistance),
                UnityEngine.Random.Range(Mathf.Clamp(-floatDistance, transform.parent.position.y, Mathf.Infinity), floatDistance),
                UnityEngine.Random.Range(-floatDistance, floatDistance)) + defaultPos.position;
        }

        //Float to it
        transform.position = Vector3.SmoothDamp(transform.position, floatTarget, ref velocity, floatSpeed);
    }

    void CheckFrustrate()
    {
        if (Input.anyKey)
        {
            frustrationTimer = frustrationIntervals; //Reset frustration
            frustrationStage = 0;
            Frustrate();
            rigidbody.velocity = Vector3.zero;
        }
        else
            frustrationTimer -= Time.deltaTime;

        if (frustrationTimer <= 0)
        {
            if (frustrationStage < 5)
            {
                frustrationStage++; //Become more frustrated
                frustrationTimer = frustrationIntervals;
                Frustrate();
            }
            else if (frustrationStage == 5)
                HitThings();
        }
    }

    void Frustrate()
    {
        //print("Frustration iteration number " + frustrationStage);

        furious = false;

        GetComponent<MeshRenderer>().material = materials[frustrationStage];
        particles.startColor = materials[frustrationStage].color;

        switch (frustrationStage)
        {
            case 0:
                floatSpeed = 3;
                floatDistance = .25f;
                break;
            case 1:
                floatSpeed = 2;
                floatDistance = .35f;
                break;
            case 2:
                floatSpeed = 1;
                floatDistance = .45f;
                break;
            case 3:
                floatSpeed = .5f;
                floatDistance = .70f;
                break;
            case 4:
                floatSpeed = .25f;
                floatDistance = 1;
                break;
            case 5:
                floatSpeed = .15f;
                floatDistance = 1.55f;
                break;
            default:
                print("ERROR in Follower.cs: Current frustration stage, " + frustrationStage + ", is out of range.");
                floatSpeed = 3;
                floatDistance = .25f;
                break;

        }

        floatDistance += .15f;
    }

    void HitThings()
    {
        furious = true;

        //Float
        if (hitGround)
        {
            floatTarget = topPos.transform.position;
            //print("Moving to defaultPos.");
            transform.position = Vector3.SmoothDamp(transform.position, floatTarget + (Vector3.up * 1.5f), ref velocity, floatSpeed);
        }
        else
        {
            floatTarget = new Vector3(
                transform.parent.position.x + UnityEngine.Random.Range(-1.5f, 1.5f),
                transform.parent.position.y - 1.5f,
                transform.parent.position.z + UnityEngine.Random.Range(-1.5f, 1.5f));
            //print("Moving to groundPos.");
            transform.position = Vector3.SmoothDamp(transform.position, floatTarget, ref velocity, floatSpeed);
        }

        //Change direction
        if (transform.position.y <= transform.parent.position.y)
        {
            //print("Hit ground.");
            if (!hitGround)
                Squeak();
            hitGround = true;
        }
        else if (transform.position.y >= topPos.position.y)
        {
            //print("Hit height.");
            hitGround = false;
        }
    }

    void Squeak()
    {
        audioManager.Squeak();
    }

    void KnockOrb()
    {
        transform.position = Vector3.MoveTowards(transform.position, orbHelpTarget.transform.position, 5 * Time.deltaTime);
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.transform.GetComponent<Orb>())
        {
            helping = false;
            orbHelpTarget.AllowCollection();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<OrbDrop>())
        {
            orbHelpTarget = other.GetComponent<OrbDrop>().orb;
            helping = true;
        }
    }
}
