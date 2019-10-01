using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class ShootHook : MonoBehaviour
{
    void Update()
    {
        //if (!isLocalPlayer) return;
        
        if (Input.GetMouseButtonDown(0))
        {
            transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.Translate(0,0,-Camera.main.transform.position.z);
        }
    }
}
