using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    
    public float moveSpeed;
    public float jumpForce;

    private Transform playerTransform;
    private Rigidbody playerRb;

    private float horizontalInput;
    private float verticalInput;

    public LayerMask whatIsGround;

    private void Start()
    {
        playerRb = GetComponent<Rigidbody>();
    }


    private void Update()
    {
        //Change facing of player according to horizontal inputs
        ChangeFacing();

        //Detect collision with Ground and make the player jump
        Jump();
    }

    private void FixedUpdate()
    {
        //Getting Horizontal input from player and adding velocity to it----Controls - A & D
        horizontalInput = Input.GetAxis("Horizontal");

        playerRb.velocity = new Vector3(playerRb.velocity.x, playerRb.velocity.y, horizontalInput * moveSpeed * Time.deltaTime);
    }

    private void ChangeFacing()
    {
        if(horizontalInput < 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if(horizontalInput > 0)
        {
            transform.rotation = Quaternion.Euler(0, 180f, 0);
        }
    }

    private void Jump()
    {
        bool isGrounded = Physics.CheckSphere(transform.position, 0.1f, whatIsGround);

        verticalInput = Input.GetAxis("Vertical");

        if (verticalInput > 0 && isGrounded)
        {
            playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }
}
