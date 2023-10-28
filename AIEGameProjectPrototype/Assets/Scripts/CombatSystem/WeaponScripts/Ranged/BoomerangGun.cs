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
            Boomerang bullet = bulletObject.GetComponent<Boomerang>();
            bullet.objectIsPooled = true;
            bullet.transform.SetPositionAndRotation(firePoint.position, Quaternion.identity);
            bullet.rigBody.velocity = fireDirection * bullet.speed;
            bullet.lifetime = _weaponData.ProjectileLifetime;
            bullet.damagePayload = damagePayload;

            bullet.shooter = transform;
        });
    }


}
