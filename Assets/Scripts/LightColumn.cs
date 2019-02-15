using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LightColumn : TriggerObject
{

    [SerializeField]
    private Transform lightEmitter;
    [SerializeField]
    private Transform lightTarget;

    [SerializeField]
    private bool activated;
    [SerializeField]
    private bool firstInPath, lastInPath;
    [SerializeField]
    private GameObject orbContainer;

    [SerializeField]
    private int pathIndex;

    public Material[] colors;

    private bool casting;

    private LineRenderer lineRend;
    
    private LightColumn activeTarget;

    void Awake()
    {
        lineRend = GetComponent<LineRenderer>();
        lineRend.SetPosition(0, lightEmitter.position);

        try
        {
            lineRend.material = colors[pathIndex];
        }
        catch (IndexOutOfRangeException)
        {
            print("ERROR in LightColumn.cs on " + name + ": Index " + pathIndex + " does not refer to a materials in the Material[] array \"colors.\"  Make sure pathIndex is no larger than the length (-1) of colors.");
        }

        if (firstInPath)
            activated = true;

        if (lastInPath)
        {
            transform.LookAt(orbContainer.transform);
            Quaternion rotation = transform.rotation;
            rotation.x = 0;
            rotation.z = 0;
            transform.rotation = rotation;

            transform.Rotate(new Vector3(0, 180, 0)); //Offset that may need to be changed after art assets are integrated
            lightTarget.transform.position = orbContainer.transform.position;
        }
    }

    void Update()
    {
        if (firstInPath && !activated)
            activated = true;
        if (activeTarget && activeTarget.pathIndex == pathIndex && !activeTarget.activated)
            activeTarget.activated = true;

        if (activated)
        {
            CastLight();
        }
        else
        {
            lineRend.SetPosition(1, lightEmitter.position);
            casting = false;
        }
    }

    public override void Trigger()
    {
        base.Trigger();

        if (activeTarget) //Check if this script is supporting/activating another (refer to CastLight function)
        {
            activeTarget.SetActiveState(false); //Deactivate any lightcolumn this is currently keeping active
            activeTarget = null; //Reset so later movements don't disrupt puzzle
        }

        transform.Rotate(new Vector3(transform.rotation.x, 45, transform.rotation.z)); //Rotate 45 degrees
        
        //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, transform.rotation.eulerAngles.y + 45, 0), Time.deltaTime * 10);
    }

    private void CastLight()
    {
        RaycastHit hit;

        if (Physics.Raycast(lightEmitter.position, lightTarget.position - lightEmitter.position, out hit, Mathf.Infinity)) //Cast ray from lightEmitter towards lightTarget
        {
            lineRend.SetPosition(0, lightEmitter.position);
            lineRend.SetPosition(1, hit.point);

            if (hit.collider.transform.parent != null) //Scripts are on top-level objects and collisions will be with bottom-level objects in hierarchy
            {
                if (hit.collider.transform.parent.GetComponent<LightColumn>() && activeTarget == null) //Check if object hit contains an instance of this script and is not already targeted
                {
                    activeTarget = hit.collider.transform.parent.GetComponent<LightColumn>(); //Get ref to that instance

                    if (activeTarget.GetPathIndex() == pathIndex) //Check if it's on the same path as this column
                        activeTarget.SetActiveState(true); //Activate it
                }
                else if (hit.collider.transform.parent.GetComponent<OrbContainer>() && lastInPath)
                    hit.collider.transform.parent.GetComponent<OrbContainer>().ActivatePath(pathIndex);
            }
        }
        else
        {
            lineRend.SetPosition(0, lightEmitter.position);
            lineRend.SetPosition(1, lightTarget.position);// * 10);
        }
    }

    public void SetActiveState(bool active)
    {
        activated = active;

        if (activeTarget && !active)
        {
            if (activeTarget.GetTarget() != this)
                activeTarget.SetActiveState(false); //Cascade deactivation only if target is not also targeting this column
            activeTarget = null;
        }
    }

    public LightColumn GetTarget()
    {
        return activeTarget;
    }

    public int GetPathIndex()
    {
        return pathIndex;
    }
}
