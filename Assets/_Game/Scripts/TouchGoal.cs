using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchGoal : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D ObjectInGoal)
    {
        Debug.Log("Level Finished by " + ObjectInGoal);
    }
}
