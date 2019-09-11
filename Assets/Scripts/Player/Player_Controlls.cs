using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Player_Controlls : NetworkBehaviour
{
    public float jumpStrength;

    public LayerMask groundLayer;

    float standardGravityScale;
    bool isGrounded;

    CircleCollider2D cc2d;
    Rigidbody2D rb2d;

    private void Awake()
    {
        cc2d = GetComponent<CircleCollider2D>();
        rb2d = GetComponent<Rigidbody2D>();

        standardGravityScale = rb2d.gravityScale;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer)
        {
            return;
        }

        //get important bools
        {
            isGrounded = Physics2D.OverlapCircle(transform.position, cc2d.radius + .1f, groundLayer);
        }

        if (isGrounded)
        {
            //stick to walls
            rb2d.velocity = Vector2.zero;
            rb2d.gravityScale = 0;

            //jump
            if (Input.GetMouseButtonDown(0))
            {
                rb2d.AddForce((Camera.main.ScreenToWorldPoint((Vector2)Input.mousePosition) - transform.position).normalized * jumpStrength, ForceMode2D.Impulse);
            }
        }
        else
        {
            rb2d.gravityScale = standardGravityScale;
        }

    }
}
