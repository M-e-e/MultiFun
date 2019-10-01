using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Player_Controlls : NetworkBehaviour
{
    public float jumpStrength;
    public float checkAngle;
    public float checkLength;

    public LayerMask groundLayer;

    float standardGravityScale;
    bool isGrounded;
    Vector3 mousePosition;
    bool anglePossible;
    


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

        //get important variables
        {
            isGrounded = Physics2D.OverlapCircle(transform.position, cc2d.radius, groundLayer);
            mousePosition = Camera.main.ScreenToWorldPoint((Vector2)Input.mousePosition);
        }

        if (isGrounded)
        {
            //stick to walls
            rb2d.velocity = Vector2.zero;
            rb2d.gravityScale = 0;

            //check jump possibility
            Vector2 toMouse = mousePosition - transform.position;
            Vector2 rightCheckPosition = Vector2.Perpendicular(toMouse).normalized * cc2d.radius;
            Vector2 leftCheckPosition = -Vector2.Perpendicular(toMouse).normalized * cc2d.radius;

            Debug.DrawRay((Vector2)transform.position + rightCheckPosition, (Quaternion.AngleAxis(-checkAngle, Vector3.forward) * toMouse).normalized * checkLength, Color.red);
            Debug.DrawRay((Vector2)transform.position + leftCheckPosition, (Quaternion.AngleAxis(checkAngle, Vector3.forward) * toMouse).normalized * checkLength, Color.red);    
            Debug.DrawRay(transform.position, toMouse, Color.red);
            
            RaycastHit2D raycastRightCheck = Physics2D.Raycast((Vector2)transform.position + rightCheckPosition, (Quaternion.AngleAxis(-checkAngle, Vector3.forward) * toMouse).normalized * checkLength);
            RaycastHit2D raycastLeftCheck = Physics2D.Raycast((Vector2)transform.position + leftCheckPosition, (Quaternion.AngleAxis(checkAngle, Vector3.forward) * toMouse).normalized * checkLength);

            anglePossible = !(raycastRightCheck && raycastRightCheck.transform.tag == "Ground" || raycastLeftCheck && raycastLeftCheck.transform.tag == "Ground");

            Debug.Log(raycastRightCheck.transform.tag + "   " + raycastLeftCheck.transform.tag);

            //jump
            if (Input.GetMouseButtonDown(0) && anglePossible)
            {
                rb2d.AddForce((mousePosition - transform.position).normalized * jumpStrength, ForceMode2D.Impulse);
            }
        }
        else
        {
            rb2d.gravityScale = standardGravityScale;
        }

    }
}
