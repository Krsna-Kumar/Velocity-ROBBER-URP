using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

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
    public string weaponName;

    [Header("Weapon Effects")]
    public ParticleSystem muzzleFlash;
    public Transform raycastOrigin;
    public ParticleSystem hitEffect;
    public TrailRenderer bulletTrail;

    [Header("Recoil")]
    public float recoilForce = 100.0f; // Adjust this value as needed
    [HideInInspector] private CinemachineVirtualCamera virtualCamera;
    [HideInInspector] public Animator rigController;


    private float coolDownTime;

    List<Bullet> bullets = new List<Bullet>();

    Ray ray;
    RaycastHit hitinfo;

    private void Awake()
    {
        virtualCamera = GameObject.Find("Virtual Camera").GetComponent<CinemachineVirtualCamera>();
    }

    Vector3 GetPosition(Bullet bullet)
    {
        //formula --- u + v*t + 0.5 * g*t*t
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
        DestroyBullets();
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

        float recoilY = UnityEngine.Random.Range(-recoilForce, recoilForce);
        Vector3 recoil = new Vector3(0, recoilY, 0f);
        Vector3 velocity = (raycastOrigin.forward.normalized * bulletSpeed) + recoil;

        //Shake Camera
        ShakeCamera();

        rigController.Play("weapon_recoil_" + weaponName, 1, 0.0f);

        var bullet = CreateBullet(raycastOrigin.position, velocity);
        bullets.Add(bullet);

    }

    void ShakeCamera()
    {
        // Generate a random impulse signal
        float shakeDuration = 0.2f; // Adjust as needed
        float shakeAmplitude = 1f; // Adjust as needed
        float shakeFrequency = 2f; // Adjust as needed

        CinemachineBasicMultiChannelPerlin noise = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        if (noise != null)
        {
            // Set the impulse parameters
            noise.m_AmplitudeGain = shakeAmplitude;
            noise.m_FrequencyGain = shakeFrequency;

            // Start the impulse signal for the specified duration
            StartCoroutine(StopShakingAfterDelay(noise, shakeDuration));
        }
    }

    private IEnumerator StopShakingAfterDelay(CinemachineBasicMultiChannelPerlin noise, float delay)
    {
        yield return new WaitForSeconds(delay);

        // Stop the impulse signal
        noise.m_AmplitudeGain = 0f;
        noise.m_FrequencyGain = 0f;
    }

    public void StopFiring()
    {
        isFiring = false;
    }
}
