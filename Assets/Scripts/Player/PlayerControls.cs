using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerControls : NetworkBehaviour
{
    public float movementSpeed;
    public float maxSpeed;
    public float jumpStrenght;

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
        rb2d.AddForce(new Vector2(Input.GetAxis("Horizontal"), 0) * movementSpeed * Time.deltaTime, ForceMode2D.Impulse);            //transform.Translate(new Vector2(Input.GetAxis("Horizontal"), 0) * movementSpeed * Time.deltaTime );

        //handle maxspeed
        if (Mathf.Abs(rb2d.velocity.x) > maxSpeed)
        {
            rb2d.velocity = new Vector2(Mathf.Clamp(rb2d.velocity.x, -maxSpeed, maxSpeed), rb2d.velocity.y);
        }

        //jump
        if ((Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)) && !jumpLock)
        {
            rb2d.AddForce(Vector2.up * jumpStrenght, ForceMode2D.Impulse);

            jumpLock = true;
        }

        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //jump reset
        if (rb2d.velocity.y <= 0)
        {
            jumpLock = false;
        }
    }
}
