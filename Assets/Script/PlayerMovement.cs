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

    private void Start()
    {
        playerRb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        //Getting Horizontal input from player and adding velocity to it----Controls - A & D
        horizontalInput = Input.GetAxis("Horizontal");

        if(horizontalInput != 0)
        {
            playerRb.velocity = new Vector3(playerRb.velocity.x, playerRb.velocity.y, horizontalInput * moveSpeed * Time.deltaTime);
        }
    }
}
