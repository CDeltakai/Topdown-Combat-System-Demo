using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : RangedWeapon
{
    [SerializeField] GameObject projectile;
    [SerializeField] Transform firePoint;

    [SerializeField] int damage = 1;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public override void OnFire()
    {

        for(int i = 0; i < weaponData.BurstCount; i++) 
        {
            float spread = Random.Range(-weaponData.Spread, weaponData.Spread);
            Vector3 fireDirection = Quaternion.Euler(0, spread, 0) * firePoint.forward;

            Bullet bullet = Instantiate(projectile, firePoint.position, firePoint.rotation).GetComponent<Bullet>();
            bullet.rigBody.velocity = fireDirection * bullet.speed;
        }


    }





}
