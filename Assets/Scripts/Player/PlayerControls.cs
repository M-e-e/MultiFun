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

    private bool isFalling = false;
    private bool isFlying = false;

    private bool jumpLock = false;

    private Animator animator;
    private Rigidbody2D rb2d;
    private SpriteRenderer spriteRenderer;
    private GameObject player_Graphics;

    private void Awake()
    {
        player_Graphics = transform.GetChild(0).gameObject;
        animator = player_Graphics.GetComponent<Animator>();
        spriteRenderer = player_Graphics.GetComponent<SpriteRenderer>();
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

            animator.SetTrigger("Jump");

            jumpLock = true;
        }

        //handle jump animation
        if (rb2d.velocity.y > 0 && !animator.GetBool("Flying"))
        {
            animator.SetBool("Flying", true);
        }
        if (rb2d.velocity.y < 0 && !animator.GetBool("Falling"))
        {
            animator.SetBool("Falling", true);
            animator.SetBool("Flying", false);
        }

        //stick to wall
        animator.SetBool("onWallRight", false);
        animator.SetBool("onWallLeft", false);

        rb2d.gravityScale = 1.5f;
        if (!Input.GetKey(KeyCode.W))
        {
            if (isWallRight)
            {
                animator.SetBool("onWallRight", true);
                //spriteRenderer.flipX = false;
                //player_Graphics.transform.position = new Vector2(.04f, player_Graphics.transform.position.y);
                if (Input.GetKey(KeyCode.D))
                {
                    rb2d.velocity = Vector2.zero;
                    rb2d.gravityScale = 0;

                }
            }
            if (isWallLeft)
            {
                animator.SetBool("onWallLeft", true);
                //spriteRenderer.flipX = true;
                //player_Graphics.transform.position = new Vector2(-.04f, player_Graphics.transform.position.y);
                if (Input.GetKey(KeyCode.A))
                {
                    rb2d.velocity = Vector2.zero;
                    rb2d.gravityScale = 0;

                }
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
            animator.SetBool("Falling", false);
            
        }

        
    }

    
    
}
