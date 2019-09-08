using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class FixCamera : NetworkBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (!isLocalPlayer)
        {
            // exit from update if this is not the local player
            return;
        }

        Camera.main.transform.position = transform.position - transform.forward * 10; // + transform.up * 3;
        Camera.main.transform.LookAt(transform.position);
        Camera.main.transform.parent = transform;
    }

}
