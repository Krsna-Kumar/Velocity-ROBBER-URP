using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed;
    public float jumpForce;

    private Rigidbody playerRb;

    private float horizontalInput;
    private bool isJumping;

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

        //Change facing of player according to horizontal inputs
        ChangeFacing();
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
            transform.Rotate(Vector3.up * 0f);
        }

        else if (dot < 0)
        {
            transform.Rotate(Vector3.up * 180f);
        }
    }

    private void Jump()
    {
        bool isGrounded = Physics.CheckSphere(transform.position, 0.1f, whatIsGround);

        isJumping = Input.GetKeyUp(KeyCode.Space);

        bool canJump = isJumping & isGrounded;

        if (canJump)
        {
            playerRb.velocity = new Vector3(playerRb.velocity.x, jumpForce, playerRb.velocity.z);
        }
    }
}
