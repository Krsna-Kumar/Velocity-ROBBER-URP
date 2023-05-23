using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastWeapon : MonoBehaviour
{
    class Bullet
    {
        public float time;
        public Vector3 initialPosition;
        public Vector3 initialVelocity;
        public TrailRenderer Tracer;
    }

    [Header("State")]
    public bool isFiring = false;

    [Header("Weapon Properties")]
    public int fireRate = 25;
    public float bulletSpeed = 1000.0f;
    public float bulletDrop = 0.0f;
    public float maxLifeTime = 3.0f;

    [Header("Weapon Effects")]
    public ParticleSystem muzzleFlash;
    public Transform raycastOrigin;
    public ParticleSystem hitEffect;
    public TrailRenderer bulletTrail;

    private float coolDownTime;

    List<Bullet> bullets = new List<Bullet>();

    Ray ray;
    RaycastHit hitinfo;

    Vector3 GetPosition(Bullet bullet)
    {
        // u + v*t + 0.5 * g*t*t
        Vector3 gravity = Vector3.down * bulletDrop;
        return (bullet.initialPosition) + (bullet.initialVelocity * bullet.time) + 0.5f * 
            gravity * bullet.time * bullet.time;
    }

    Bullet CreateBullet(Vector3 position , Vector3 velocity)
    {
        Bullet bullet = new Bullet();
        bullet.initialPosition = position;
        bullet.initialVelocity = velocity;
        bullet.time = 0.0f;
        bullet.Tracer = Instantiate(bulletTrail, position, Quaternion.identity);
        bullet.Tracer.AddPosition(position);
        return bullet;
    }
    public void StartFiring()
    {
        isFiring = true;
        coolDownTime = 0.0f;
        FireBullet();

    }

    public void UpdateFiring(float deltatime)
    {
        coolDownTime += deltatime;

        float fireInterval = 1.0f / fireRate;
        while(coolDownTime > 0.0f)
        {
            FireBullet();
            coolDownTime -= fireInterval;
        }
    }

    public void UpdateBullets(float deltatime)
    {
        SimulateBullets(deltatime);
    }

    void SimulateBullets(float deltatime)
    {
        bullets.ForEach(bullets =>
        {
            Vector3 p0 = GetPosition(bullets);
            bullets.time += deltatime;
            Vector3 p1 = GetPosition(bullets);
            RaycastSegment(p0, p1, bullets);
        });
    }

    void DestroyBullets()
    {
        bullets.RemoveAll(bullets => bullets.time >= maxLifeTime);
    }

    void RaycastSegment(Vector3 start, Vector3 end, Bullet bullet)
    {
        Vector3 Direction = end - start;
        float distance = Direction.magnitude;
        ray.origin = start;
        ray.direction = Direction;
        if (Physics.Raycast(ray, out hitinfo, distance))
        {
            //Debug.DrawLine(raycastOrigin.position, hitinfo.point, Color.red, 1.0f);

            hitEffect.transform.position = hitinfo.point;
            hitEffect.transform.forward = hitinfo.normal;
            hitEffect.Emit(1);

            bullet.Tracer.transform.position = hitinfo.point;
            bullet.time = maxLifeTime;
        }
        else
        {
            bullet.Tracer.transform.position = end;
        }
    }

    private void FireBullet()
    {
        muzzleFlash.Emit(1);

        Vector3 velocity = raycastOrigin.forward.normalized * bulletSpeed ;

        var bullet = CreateBullet(raycastOrigin.position, velocity);
        bullets.Add(bullet);

        ////Casting Ray to Detect the collision with other objects
        //ray.origin = raycastOrigin.position;
        //ray.direction = raycastOrigin.forward;

        //var tracer = Instantiate(bulletTrail, ray.origin, Quaternion.identity);
        //tracer.AddPosition(ray.origin);

        //if (Physics.Raycast(ray, out hitinfo))
        //{
        //    //Debug.DrawLine(raycastOrigin.position, hitinfo.point, Color.red, 1.0f);

        //    hitEffect.transform.position = hitinfo.point;
        //    hitEffect.transform.forward = hitinfo.normal;
        //    hitEffect.Emit(1);

        //    tracer.transform.position = hitinfo.point;
        //}
    }

    public void StopFiring()
    {
        isFiring = false;
    }
}
