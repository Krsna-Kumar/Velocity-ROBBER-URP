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

    private Animator anim;
    private AnimatorOverrideController overrideController;

    private void Start()
    {
        anim = GetComponent<Animator>();
        overrideController = anim.runtimeAnimatorController as AnimatorOverrideController;

        RaycastWeapon existingWeapon = GetComponentInChildren<RaycastWeapon>();
        if (existingWeapon)
        {
            Equip(existingWeapon);
        }
    }

    private void Update()
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
        }
        else
        {
            handIK.weight = 0.0f;
            anim.SetLayerWeight(1, 0);
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
        handIK.weight = 1.0f;
        anim.SetLayerWeight(1, 1);

        Invoke(nameof(SetAnimationDelayed), 0.001f);

    }

    void SetAnimationDelayed()
    {
        overrideController["weapon_Anim_empty"] = weapon.weaponAnimation;

    }

    [ContextMenu("Save Menu Pose")]
    void SaveWeaponPose()
    {
        GameObjectRecorder recorder = new GameObjectRecorder(gameObject);
        recorder.BindComponentsOfType<Transform>(weaponParent.gameObject, false);
        recorder.BindComponentsOfType<Transform>(weaponLeftGrip.gameObject, false);
        recorder.BindComponentsOfType<Transform>(weaponRightGrip.gameObject, false);
        recorder.TakeSnapshot(0.0f);
        recorder.SaveToClip(weapon.weaponAnimation);
    }
}
