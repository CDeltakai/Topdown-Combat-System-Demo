using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomerangGun : RangedWeapon
{

    protected override void InitializeProjectile()
    {
        float spread = Random.Range(-_weaponData.Spread, _weaponData.Spread);
        Vector3 fireDirection = Quaternion.Euler(0, spread, 0) * firePoint.forward;

        GameObject bulletObject = projectilePool.UseObject(bulletObject =>
        {
            Boomerang boomerang = bulletObject.GetComponent<Boomerang>();
            if(!boomerang)
            {
                Debug.LogError(gameObject.name + " is incompatible with projectile prefab: " + bulletObject.name + ", BoomerangGun weapon data must be assigned weapon data " +
                "which assigns a projectile prefab that has component Boomerang.");
                return;
            }

            boomerang.objectIsPooled = true;
            boomerang.transform.position = firePoint.position;
            boomerang.rigBody.velocity = fireDirection * boomerang.speed;
            boomerang.lifetime = _weaponData.ProjectileLifetime;
            boomerang.damagePayload = damagePayload;

            boomerang.shooter = transform;

            boomerang.OnReturned += BoomerangReturned;
        });
    }

    void BoomerangReturned()
    {
        RestoreAmmoToReserve(1);
    }

}
