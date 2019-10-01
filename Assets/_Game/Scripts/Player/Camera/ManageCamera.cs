using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class ManageCamera : MonoBehaviour
{
    private void Start()
    {
        Camera.main.transform.parent = gameObject.transform;
        
        Debug.Log("START");
    }

}
