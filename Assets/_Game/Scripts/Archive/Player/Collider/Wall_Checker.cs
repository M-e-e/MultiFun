using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Wall_Checker : NetworkBehaviour
{
    public bool isWalled;

    private void FixedUpdate()
    {
        isWalled = false;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        isWalled = true;
    }
}
