using UnityEngine;
using UnityEngine.Animations.Rigging;

public class AimTarget : MonoBehaviour
{
    public float speed = 0.1f; // speed of the lerp
    public float zPosition = 0f; // the z position of the object
    public float aimDuration = 0.3f;
    [Space]
    public Rig weaponAimRigLayer;

    private Camera mainCamera;
    private RaycastWeapon weapon;


    void Awake()
    {
        mainCamera = Camera.main;
    }

    private void Start()
    {
        weapon = GameObject.FindWithTag("Weapon").GetComponent<RaycastWeapon>();
    }

    void FixedUpdate()
    {
        //Get the mouse position
        Vector3 mousePosition = Input.mousePosition;

        //Convert the mouse position to world coordinates
        Vector3 targetPosition = mainCamera.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, zPosition));

        //Interpolate the position of the object towards the target position
        transform.position = Vector3.Lerp(transform.position, targetPosition, speed);
        //transform.position = targetPosition;


    }

    private void LateUpdate()
    {
        //if (Input.GetMouseButton(1))
        //{
        //    weaponAimRigLayer.weight += Time.deltaTime / aimDuration;
        //}
        //else
        //{
        //    weaponAimRigLayer.weight -= Time.deltaTime / aimDuration;
        //}

        if (weaponAimRigLayer)
        {
            weaponAimRigLayer.weight = 1.0f;
        }

        if (weapon)
        {
            if (Input.GetMouseButtonDown(0))
            {
                weapon.StartFiring();
            }

            if (weapon.isFiring)
            {
                weapon.UpdateFiring(Time.deltaTime);
            }

            weapon.UpdateBullets(Time.deltaTime);

            if (Input.GetMouseButtonUp(0))
            {
                weapon.StopFiring();
            }
        }

        
    }

    

}
