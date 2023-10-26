using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public abstract class RangedWeapon : MonoBehaviour
{
    [SerializeField] protected Transform firePoint;
    [SerializeField] protected RangedWeaponSO weaponData;
    [SerializeField] ParticleSystem muzzleFlash;
    protected DamagePayload damagePayload;

    protected CinemachineImpulseSource cinemachineImpulseSource;


    protected int magazineCapacity;
    protected int currentMagazine;

    protected int reserveCapacity;
    protected int currentReserve;




    protected virtual void Awake() 
    {
        damagePayload = weaponData.Payload;

        magazineCapacity = weaponData.MagazineCapacity;
        currentMagazine = magazineCapacity;

        reserveCapacity = weaponData.ReserveCapacity;
        currentReserve = reserveCapacity;

        if(weaponData.MuzzleFlashPrefab)
        {
            muzzleFlash = Instantiate(weaponData.MuzzleFlashPrefab, firePoint).GetComponent<ParticleSystem>();
        }

        cinemachineImpulseSource = GetComponent<CinemachineImpulseSource>();

    }

    protected virtual void Start()
    {

    }


    public virtual void OnFire()
    {
        for(int i = 0; i < weaponData.BurstCount; i++) 
        {
            float spread = Random.Range(-weaponData.Spread, weaponData.Spread);
            Vector3 fireDirection = Quaternion.Euler(0, spread, 0) * firePoint.forward;

            Bullet bullet = Instantiate(weaponData.ProjectilePrefab, firePoint.position, firePoint.rotation).GetComponent<Bullet>();
            bullet.rigBody.velocity = fireDirection * bullet.speed;
            bullet.lifetime = weaponData.ProjectileLifetime;
            bullet.damagePayload = damagePayload;
        }

        if(muzzleFlash)
        {
            muzzleFlash.Play();
        }

        if(cinemachineImpulseSource)
        {
            cinemachineImpulseSource.m_DefaultVelocity = new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), 0);

            cinemachineImpulseSource.m_ImpulseDefinition.m_ImpulseDuration = weaponData.CameraShakeDuration;
            cinemachineImpulseSource.GenerateImpulseWithForce(weaponData.CameraShakeMagnitude);
        }

    }

    public virtual void Reload(){}




}
