using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerControls : NetworkBehaviour
{
    public float movementSpeed;
    public float maxSpeed;
    public float jumpStrength;

    public float raycastGroundedDistance;
    public float raycastWallDistance;
    public LayerMask raycastLayer;

    private bool isGrounded;
    private bool isWallLeft;
    private bool isWallRight;

    private bool jumpLock = false;

    private Rigidbody2D rb2d;

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
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
            // exit from update if this is not the local player
            return;
        }

        //left-right movement
        if (!isGrounded)
        {
            rb2d.AddForce(new Vector2(Input.GetAxis("Horizontal"), 0) * movementSpeed * Time.deltaTime, ForceMode2D.Impulse);            //transform.Translate(new Vector2(Input.GetAxis("Horizontal"), 0) * movementSpeed * Time.deltaTime );
        }


        //handle maxspeed
        if (Mathf.Abs(rb2d.velocity.x) > maxSpeed)
        {
            rb2d.velocity = new Vector2(Mathf.Clamp(rb2d.velocity.x, -maxSpeed, maxSpeed), rb2d.velocity.y);
        }

        //jump
        if (Input.GetKey(KeyCode.W) && !jumpLock && (isGrounded || isWallLeft || isWallRight))
        {

            rb2d.AddForce(Vector2.up * jumpStrength, ForceMode2D.Impulse);

            if (isWallLeft)
            {
                rb2d.AddForce(Vector2.right * jumpStrength, ForceMode2D.Impulse);
            }
            if (isWallRight)
            {
                rb2d.AddForce(Vector2.left * jumpStrength, ForceMode2D.Impulse);
            }

            jumpLock = true;
        }

        //stick to wall
        rb2d.gravityScale = 1.5f;
        if (!Input.GetKey(KeyCode.W))
        {
            if (Input.GetKey(KeyCode.D) && isWallRight)
            {
                rb2d.velocity = Vector2.zero;
                rb2d.gravityScale = 0;
            }
            if (Input.GetKey(KeyCode.A) && isWallLeft)
            {
                rb2d.velocity = Vector2.zero;
                rb2d.gravityScale = 0;
            }
        }
        
        isGrounded = false;
        isWallLeft = false;
        isWallRight = false;

        RaycastHit2D hitGround = Physics2D.Raycast(transform.position, Vector2.down, raycastGroundedDistance, raycastLayer);
        Debug.DrawRay(transform.position, Vector2.down * raycastGroundedDistance, Color.red);

        RaycastHit2D hitLeftWall = Physics2D.Raycast(transform.position, Vector2.left, raycastWallDistance, raycastLayer);
        Debug.DrawRay(transform.position, Vector2.left * raycastWallDistance, Color.red);

        RaycastHit2D hitRightWall = Physics2D.Raycast(transform.position, Vector2.right, raycastWallDistance, raycastLayer);
        Debug.DrawRay(transform.position, Vector2.right * raycastWallDistance, Color.red);


        if (hitGround)
        {
            //check grounded
            if (hitGround.transform.tag == "Ground")
            {
                //Debug.Log(hitGround.transform.tag);
                isGrounded = true;
            }
        }

        if (hitLeftWall)
        {
            //check left wall
            if (hitLeftWall.transform.tag == "Ground")
            {
                isWallLeft = true;
            }
        }

        if (hitRightWall)
        {
            //check right wall
            if (hitRightWall.transform.tag == "Ground")
            {
                isWallRight = true;
            }
        }

        
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        //jump reset
        if (rb2d.velocity.y <= 0)
        {
            jumpLock = false;
        }


    }
}
