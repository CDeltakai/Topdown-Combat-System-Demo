using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RangedWeapon : MonoBehaviour
{
    [SerializeField] protected Transform firePoint;
    [SerializeField] protected RangedWeaponSO weaponData;
    protected DamagePayload damagePayload;

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

    }




    public virtual void OnFire()
    {
        for(int i = 0; i < weaponData.BurstCount; i++) 
        {
            float spread = Random.Range(-weaponData.Spread, weaponData.Spread);
            Vector3 fireDirection = Quaternion.Euler(0, spread, 0) * firePoint.forward;

            Bullet bullet = Instantiate(weaponData.ProjectilePrefab, firePoint.position, firePoint.rotation).GetComponent<Bullet>();
            bullet.rigBody.velocity = fireDirection * bullet.speed;
        }
    }

    public virtual void Reload(){}





}
