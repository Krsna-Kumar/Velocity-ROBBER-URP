using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastWeapon : MonoBehaviour
{
    [Header("State")]
    public bool isFiring = false;

    [Header("Weapon Properties")]
    public ParticleSystem muzzleFlash;
    public Transform raycastOrigin;
    public ParticleSystem hitEffect;

    Ray ray;
    RaycastHit hitinfo;
    public void StartFiring()
    {
        isFiring = true;
        muzzleFlash.Emit(1);

        //Casting Ray to Detect the collision with other objects
        ray.origin = raycastOrigin.position;
        ray.direction = raycastOrigin.forward;

        if(Physics.Raycast(ray, out hitinfo))
        {
            //Debug.DrawLine(raycastOrigin.position, hitinfo.point, Color.red, 1.0f);

            hitEffect.transform.position = hitinfo.point;
            hitEffect.transform.forward = hitinfo.normal;
            hitEffect.Emit(1);
        }


    }

    public void StopFiring()
    {
        isFiring = false;
    }
}
