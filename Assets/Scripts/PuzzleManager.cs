using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    public GameObject gate;
    private int orbsCollected = 0;

    public void CollectOrb()
    {
        orbsCollected++;

        if (orbsCollected == 3)
            Destroy(gate);
    }
}
