using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Animations;

public class ActiveWeapon : MonoBehaviour
{
    private RaycastWeapon weapon;

    public UnityEngine.Animations.Rigging.Rig handIK;

    public Transform weaponParent;

    public Transform weaponLeftGrip;
    public Transform weaponRightGrip;

    public Animator rigController;
    

    private void Start()
    {
        

        RaycastWeapon existingWeapon = GetComponentInChildren<RaycastWeapon>();
        if (existingWeapon)
        {
            Equip(existingWeapon);
        }
    }

    private void LateUpdate()
    {
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

            if (Input.GetKeyDown(KeyCode.X))
            {
                bool unequipWeapons = rigController.GetBool("unquipWeapon");

                rigController.SetBool("unquipWeapon", !unequipWeapons);
            }
        }
       
    }

    public void Equip(RaycastWeapon newWeapon)
    {
        if (weapon)
        {
            Destroy(weapon.gameObject);
        }
        weapon = newWeapon;

        weapon.transform.parent = weaponParent;
        weapon.transform.localPosition = Vector3.zero;
        weapon.transform.localRotation = Quaternion.identity;
        rigController.Play("equip_" + weapon.weaponName);
    }

    
}
