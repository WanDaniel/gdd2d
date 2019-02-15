using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    #region Variables
    //Character movement
    /// <summary>
    /// Player movement speed.
    /// </summary>
    public float defaultSpeed;
    private float speed;
    /// <summary>
    /// Player jump force.
    /// </summary>
    public float jumpSpeed = 4.0f;
    private Rigidbody rb;

    //Character rotation
    /// <summary>
    /// Used to rotate character based on camera rotation.
    /// </summary>
    private Vector3 moveDirection = Vector3.zero;
    /// <summary>
    /// Used to determine direction.  1 is forward movement, 0 is idle, -1 is backward movement.
    /// </summary>
    private int velocityForward;
    /// <summary>
    /// Used to determine direction.  1 is right movement, 0 is idle, -1 is left movement.
    /// </summary>
    private int velocityRight;

    //Character rotation
    /// <summary>
    /// Forward direction of the camera.
    /// </summary>
    private Vector3 forward = Vector3.zero;
    /// <summary>
    /// Right direction of the camera.
    /// </summary>
    private Vector3 right = Vector3.zero;

    bool plateActivated;
    #endregion

    void Awake()
    {
        try
        {
            rb = GetComponent<Rigidbody>();
        }
        catch (MissingComponentException)
        {
            print("ERROR in PlayerController.cs: The object \"" + name + "\" does not have a Rigidbody component attached to it.");
        }
    }

    void FixedUpdate()
    {
        Movement();

        if (!Input.GetKey(KeyCode.LeftAlt))
            Orientation();
    }

    void Movement()
    {
        float inputHoriz = Input.GetAxis("Horizontal");
        float inputVert = Input.GetAxis("Vertical");

        if (inputHoriz != 0 || inputVert != 0)
        {
            if (Math.Abs(inputHoriz) > Math.Abs(inputVert)) //Check if absolute value of horiz input is greater than that of vert
                speed = Math.Abs(defaultSpeed * Math.Abs(inputHoriz) - (defaultSpeed * .35f)); //Set speed based on horiz input and reduce due to strafing
            else if (inputVert < 0)
                speed = Math.Abs(defaultSpeed * Math.Abs(inputVert) - (defaultSpeed * .35f)); //Set speed based on vert input and reduce due to backwards direction
            else
                speed = Math.Abs(defaultSpeed * Math.Abs(inputVert)); //Set speed based on vert input
        }

        #region Physical movement
        Vector3 velocity;

        if (Input.GetKey(KeyCode.LeftAlt))
            velocity = transform.position + ((inputVert * transform.forward) + (inputHoriz * transform.right)) * speed * Time.deltaTime; //Move based on character rotation
        else
            velocity = transform.position + ((inputVert * Camera.main.transform.forward) + (inputHoriz * Camera.main.transform.right)) * speed * Time.deltaTime; //Move based on camera rotation

        rb.MovePosition(velocity);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.velocity += jumpSpeed * Vector3.up;
        }
        #endregion

        #region Discrete movement
        //if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        //{
        //    transform.position += Camera.main.transform.forward * speed * Time.deltaTime;
        //}
        //if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        //{
        //    transform.position += -Camera.main.transform.right * speed * Time.deltaTime;
        //}
        //if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        //{
        //    transform.position += -Camera.main.transform.forward * speed * Time.deltaTime;
        //}
        //if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        //{
        //    transform.position += Camera.main.transform.right * speed * Time.deltaTime;
        //}
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    rb.velocity += jumpSpeed * Vector3.up;
        //}
        #endregion
    }

    void Orientation()
    {
        Quaternion characterRotation = Camera.main.transform.rotation;
        characterRotation.x = 0;
        characterRotation.z = 0;

        transform.rotation = Quaternion.Lerp(transform.rotation, characterRotation, 10 * Time.deltaTime);
    }

    //void Orientation()
    //{
    //    forward = new Vector3(0, Camera.main.transform.forward.y, 0);
    //    right = new Vector3(0, Camera.main.transform.right.y, 0);

    //    //forward = new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z);
    //    //right = new Vector3(Camera.main.transform.right.x, 0, Camera.main.transform.right.z);

    //    forward += transform.forward;
    //    right += transform.right;

    //    #region Read vertical input
    //    if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
    //    {
    //        velocityForward = 1;
    //        moveDirection += forward;
    //    }
    //    else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
    //    {
    //        velocityForward = -1;
    //        moveDirection += -forward;
    //    }
    //    else
    //    {
    //        velocityForward = 0;
    //    }
    //    #endregion

    //    #region Read horizontal input
    //    if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
    //    {
    //        velocityRight = 1;
    //        moveDirection += right;
    //    }
    //    else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
    //    {
    //        velocityRight = -1;
    //        moveDirection += -right;
    //    }
    //    else
    //    {
    //        velocityRight = 0;
    //    }
    //    #endregion

    //    #region Determine direction (Physical)
    //    if (velocityForward == 1 && velocityRight == 1)
    //        rb.rotation = Quaternion.Euler(-forward - right);
    //    else if (velocityForward == 1 && velocityRight == 0)
    //        rb.rotation = Quaternion.Euler(-forward);
    //    else if (velocityForward == 1 && velocityRight == -1)
    //        rb.rotation = Quaternion.Euler(-forward + right);
    //    else if (velocityForward == 0 && velocityRight == 1)
    //        rb.rotation = Quaternion.Euler(-right);
    //    //No case for forward==0 && right==0 because this should stop without rotating
    //    else if (velocityForward == 0 && velocityRight == -1)
    //        rb.rotation = Quaternion.Euler(right);
    //    else if (velocityForward == -1 && velocityRight == 1)
    //        rb.rotation = Quaternion.Euler(forward - right);
    //    else if (velocityForward == -1 && velocityRight == 0)
    //        rb.rotation = Quaternion.Euler(forward);
    //    else if (velocityForward == -1 && velocityRight == -1)
    //        rb.rotation = Quaternion.Euler(forward + right);
    //    else if (velocityForward != 0 && velocityRight != 0)
    //        print("ERROR in PlayerController.cs: Rotation values are unreachable.  Vertical value " + velocityForward + ".  Horizontal value " + velocityRight);
    //    #endregion

    //    #region Determine direction (Discrete)
    //    //if (velocityForward == 1 && velocityRight == 1)
    //    //    moveDirection = -forward - right;
    //    //else if (velocityForward == 1 && velocityRight == 0)
    //    //    moveDirection = -forward;
    //    //else if (velocityForward == 1 && velocityRight == -1)
    //    //    moveDirection = -forward + right;
    //    //else if (velocityForward == 0 && velocityRight == 1)
    //    //    moveDirection = -right;
    //    ////No case for forward==0 && right==0 because this should stop without rotating
    //    //else if (velocityForward == 0 && velocityRight == -1)
    //    //    moveDirection = right;
    //    //else if (velocityForward == -1 && velocityRight == 1)
    //    //    moveDirection = forward - right;
    //    //else if (velocityForward == -1 && velocityRight == 0)
    //    //    moveDirection = forward;
    //    //else if (velocityForward == -1 && velocityRight == -1)
    //    //    moveDirection = forward + right;
    //    //else if (velocityForward != 0 && velocityRight != 0)
    //    //    print("ERROR in PlayerController.cs: Rotation values are unreachable.  Vertical value " + velocityForward + ".  Horizontal value " + velocityRight);
    //    #endregion

    //    //transform.LookAt(transform.position - moveDirection);
    //}

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PressurePlate>() && !plateActivated)
        {
            other.GetComponent<PressurePlate>().Activate();
            plateActivated = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<PressurePlate>())
        {
            other.GetComponent<PressurePlate>().ResetPlate();
            plateActivated = false;
        }
    }
}
