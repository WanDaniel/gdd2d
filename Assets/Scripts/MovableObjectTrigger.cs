using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableObjectTrigger : MonoBehaviour
{
    public int path;
    [SerializeField]
    private OrbContainer orbContainer;

    public void Activate()
    {
        orbContainer.ActivatePath(path);
        print("Path " + path + " activated.");
    }
}
