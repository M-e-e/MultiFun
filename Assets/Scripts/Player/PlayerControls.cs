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
    private bool onWallLeft;
    private bool onWallRight;

    public bool jumpLock = false;

    private Animator animator;
    private Rigidbody2D rb2d;
    private SpriteRenderer spriteRenderer;
    private GameObject player_Graphics;
    private CapsuleCollider2D capsuleCollider2D;
    private EdgeCollider2D edgeCollider2D;    

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

        //reset all bools
        {
            isGrounded = false;
            onWallLeft = false;
            onWallRight = false;
        }

        //raycast - check ground and walls
        {
            Vector3 raycastOffsetGround = new Vector2(.6f, 0);
            RaycastHit2D hitGround1 = Physics2D.Raycast(transform.position + raycastOffsetGround, Vector2.down, raycastGroundedDistance, raycastLayer);
            RaycastHit2D hitGround2 = Physics2D.Raycast(transform.position - raycastOffsetGround, Vector2.down, raycastGroundedDistance, raycastLayer);
            Debug.DrawRay(transform.position + raycastOffsetGround, Vector2.down * raycastGroundedDistance, Color.red);
            Debug.DrawRay(transform.position - raycastOffsetGround, Vector2.down * raycastGroundedDistance, Color.red);

            RaycastHit2D hitLeftWall = Physics2D.Raycast(transform.position, Vector2.left, raycastWallDistance, raycastLayer);
            Debug.DrawRay(transform.position, Vector2.left * raycastWallDistance, Color.red);

            RaycastHit2D hitRightWall = Physics2D.Raycast(transform.position, Vector2.right, raycastWallDistance, raycastLayer);
            Debug.DrawRay(transform.position, Vector2.right * raycastWallDistance, Color.red);

            isGrounded = hitGround1 && hitGround1.transform.tag == "Ground";
            isGrounded = hitGround2 && hitGround2.transform.tag == "Ground";

            onWallLeft = hitLeftWall && hitLeftWall.transform.tag == "Ground";

            onWallRight = hitRightWall && hitRightWall.transform.tag == "Ground";     
        }

        //stick to wall
        {
            rb2d.gravityScale = 1.5f;
            if (!Input.GetKey(KeyCode.W))
            {
                if (onWallRight)
                {
                    if (Input.GetKey(KeyCode.D))
                    {
                        rb2d.velocity = Vector2.zero;
                        rb2d.gravityScale = 0;
                    }
                }
                else if (onWallLeft)
                {
                    if (Input.GetKey(KeyCode.A))
                    {
                        rb2d.velocity = Vector2.zero;
                        rb2d.gravityScale = 0;

                    }
                }
            }
        }

        //handle animation
        {
            animator.SetBool("isGrounded", isGrounded);
            animator.SetBool("onWallRight", onWallRight);
            animator.SetBool("onWallLeft", onWallLeft);
            animator.SetFloat("SpeedY", (onWallLeft || onWallRight) ? 0 : rb2d.velocity.y);
        }    
    }

    private void FixedUpdate()
    {
        //left-right movement
        {
            if (!isGrounded)
            {
                rb2d.AddForce(new Vector2(Input.GetAxis("Horizontal"), 0) * movementSpeed * Time.deltaTime, ForceMode2D.Impulse);
            }
        }

        //handle maxspeed
        {
            if (Mathf.Abs(rb2d.velocity.x) > maxSpeed)
            {
                rb2d.velocity = new Vector2(Mathf.Clamp(rb2d.velocity.x, -maxSpeed, maxSpeed), rb2d.velocity.y);
            }
        }

        //jump
        {
            if (Input.GetKey(KeyCode.W) && !jumpLock && (isGrounded || onWallLeft || onWallRight))
            {

                rb2d.AddForce(Vector2.up * jumpStrength, ForceMode2D.Impulse);

                if (onWallLeft)
                {
                    rb2d.AddForce(Vector2.right * jumpStrength, ForceMode2D.Impulse);
                }
                if (onWallRight)
                {
                    rb2d.AddForce(Vector2.left * jumpStrength, ForceMode2D.Impulse);
                }

                jumpLock = true;
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
