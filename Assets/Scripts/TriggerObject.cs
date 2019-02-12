using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerObject : MonoBehaviour {

    public virtual void Trigger()
    {
        print("Base trigger function running on " + name + ".");
    }
}
