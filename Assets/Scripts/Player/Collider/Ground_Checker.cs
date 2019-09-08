using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Ground_Checker : NetworkBehaviour
{
    public bool isGrounded;

    private void FixedUpdate()
    {
        isGrounded = false;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        isGrounded = true;
    }
}
