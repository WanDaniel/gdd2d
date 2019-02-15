using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbContainer : MonoBehaviour {

    public bool testCrumble;

    private bool orbRevealed;
    private bool bricksDestroyed;

    public int numberOfPaths;
    private bool[] pathsActive;

    public GameObject orb;
    public GameObject[] partsToDestroy;

    private Material[] mats;

	void Awake () {
        pathsActive = new bool[numberOfPaths];

        mats = new Material[partsToDestroy.Length];

        for (int i = 0; i < partsToDestroy.Length; i++)
        {
            try
            {
                mats[i] = partsToDestroy[i].GetComponent<MeshRenderer>().material;
            }
            catch (MissingComponentException)
            {
                print("ERROR in OrbContainer.cs: " + partsToDestroy[i].name + " does not have a MeshRenderer component or is missing a material.");
            }
        }
    }

    void Update()
    {
        if (testCrumble && !orbRevealed)
            Crumble();

        if (orbRevealed && !bricksDestroyed)
            FadeOutBricks();
    }

    public void ActivatePath(int pathNum)
    {
        pathsActive[pathNum] = true;

        if (CheckPaths() && !orbRevealed)
            Crumble();
    }

    bool CheckPaths()
    {
        bool temp = true;

        for (int i = 0; i < pathsActive.Length; i++)
        {
            if (pathsActive[i] == false)
            {
                temp = false;
                break;
            }
        }

        return temp;
    }

    void Crumble()
    {
        print("Crumbling");

        orbRevealed = true;

        for (int i = 0; i < partsToDestroy.Length; i++)
        {
            //Force bricks away from orb
            Rigidbody rb = partsToDestroy[i].GetComponent<Rigidbody>();
            rb.useGravity = true;
            rb.isKinematic = false;
            Vector3 direction = partsToDestroy[i].transform.position - orb.transform.position;
            rb.AddForce(direction * 5, ForceMode.Impulse);
        }
    }

    void FadeOutBricks()
    {
        for (int i = 0; i < partsToDestroy.Length; i++)
        {
            Color newColor = mats[i].color;
            newColor.a -= Time.deltaTime / 2;
            mats[i].color = newColor;
            partsToDestroy[i].GetComponent<MeshRenderer>().material = mats[i];

            if (newColor.a <= 0)
            {
                foreach (GameObject brick in partsToDestroy)
                    Destroy(brick);

                bricksDestroyed = true; //Prevent recursion after bricks are destroyed
                break;
            }
        }

        //foreach (GameObject brick in partsToDestroy)
        //{
        //    //Fade bricks out of existence
        //    Material mat = brick.GetComponent<MeshRenderer>().material;
        //    Color color = mat.color;
        //    color.a -= Time.deltaTime;
        //    mat.color = color;

        //    if (color.a <= 10)
        //    {
        //        Destroy(brick);
        //        bricksDestroyed = true; //Prevent recursion after bricks are destroyed
        //    }
        //}
    }
}
