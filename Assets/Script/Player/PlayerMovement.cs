using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    
    public float moveSpeed;
    public float jumpForce;

    private Rigidbody playerRb;

    private float horizontalInput;
    private float verticalInput;

    public GameObject targetTransform;
    public LayerMask whatIsGround;

 

    //Player Animation Booleans
    [HideInInspector]
    public float WalkDetect;

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

        if(horizontalInput != 0f)
        {
            WalkDetect = 1;
        }
        else
        {
            WalkDetect = 0;
        }
    }

    private void ChangeFacing()
    {
        /*if(horizontalInput > 0)
        {
            // transform.rotation = Quaternion.Euler(0, 0, 0);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, 0), 0.35f);
        }
        else if(horizontalInput < 0)
        {
            // transform.rotation = Quaternion.Euler(0, 180f, 0);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 180f, 0), 0.35f);
        }*/
        Vector3 heading = targetTransform.transform.position - transform.position;
        float dot = Vector3.Dot(heading, transform.forward);

        if (dot > 0)
        {
            Debug.Log("Target is in front of the player.");
        }
        else if (dot < 0)
        {
            Debug.Log("Target is behind the player.");
        }
        else
        {
            Debug.Log("Target is exactly in line with the player's forward direction.");
        }
    }

    private void Jump()
    {
        bool isGrounded = Physics.CheckSphere(transform.position, 0.1f, whatIsGround);

        verticalInput = Input.GetAxis("Vertical");

        if (verticalInput > 0 && isGrounded)
        {
            playerRb.velocity = new Vector3(playerRb.velocity.x, jumpForce, playerRb.velocity.z);
        }
    }
}
