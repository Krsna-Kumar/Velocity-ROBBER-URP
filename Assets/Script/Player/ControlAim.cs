using UnityEngine;

public class ControlAim : MonoBehaviour
{
    public Transform playerTarget;

    public Vector3 extraOffset;

    // Rotation speed
    public float rotationSpeed = 10.0f;

    // Smooth time
    public float smoothTime = 0.3f;

    public Joystick aimJoystick;


    public float speed = 2f; // Speed of motion
    public float radius = 3f; // Radius of circular motion
    private float angle = 0f; // Current angle in radians

    private void FixedUpdate()
    {
        //function to follow player
        FollowPlayer();

        //Control the rotation of this object on x-axis
        ControlRotation();
    }

    private void FollowPlayer()
    {
        if (playerTarget != null)
        {
            transform.position = playerTarget.position + extraOffset;
        }
    }

    private void ControlRotation()
    {
        /*// Get the mouse position
        float mouseX = Input.mousePosition.x;

        // Calculate the rotation angle
        float rotationAngle = (mouseX / Screen.width) * 360 - 180;

        // Calculate the target rotation
        Quaternion targetRotation = Quaternion.Euler(rotationAngle, 0, 0) * Quaternion.Euler(0, transform.rotation.y, transform.rotation.z);

        // Interpolate between the current rotation and the target rotation
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, smoothTime * Time.deltaTime * rotationSpeed);*/


        float mouseX = Input.mousePosition.x;
        float mouseY = Input.mousePosition.y;

            angle += speed * Time.deltaTime; // Increase angle over time
            float x = Mathf.Cos(angle) * radius; // X position based on angle and radius
            float y = Mathf.Sin(angle) * radius; // Y position based on angle and radius
            transform.position = new Vector3(x * mouseX, y * mouseY, 0f); // Set the position of the game object
        }
    }

