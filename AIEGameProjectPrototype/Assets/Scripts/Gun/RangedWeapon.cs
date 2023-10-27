using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

public abstract class RangedWeapon : MonoBehaviour
{
    [SerializeField] protected Transform firePoint;
    [SerializeField] protected RangedWeaponDataSO weaponData;
    [SerializeField] ParticleSystem muzzleFlash;
    protected DamagePayload damagePayload;
    protected CinemachineImpulseSource cinemachineImpulseSource;
    //protected ObjectPool projectilePool;


    protected int magazineCapacity;
    protected int currentMagazine;

    protected int reserveCapacity;
    protected int currentReserve;


    bool canFire = true;


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
        CreateProjectilePool();
    }

    void CreateProjectilePool()
    {
        GameObject newProjectilePool = new(weaponData.WeaponName +"ProjectilePool");
        //newProjectilePool.AddComponent<ObjectPool>();
        //projectilePool = newProjectilePool.GetComponent<ObjectPool>();
    }

    public virtual void OnFire()
    {

        if(!canFire){ return; }

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


        StartCoroutine(Cooldown(weaponData.FireRate));


        if(cinemachineImpulseSource)
        {
            cinemachineImpulseSource.m_DefaultVelocity = new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), 0);

            cinemachineImpulseSource.m_ImpulseDefinition.m_ImpulseDuration = weaponData.CameraShakeDuration;
            cinemachineImpulseSource.GenerateImpulseWithForce(weaponData.CameraShakeMagnitude);
        }

    }

    public virtual void Reload(){}


    protected IEnumerator Cooldown(float duration)
    {
        canFire = false;
        yield return new WaitForSeconds(duration);
        canFire = true;
    }


}
