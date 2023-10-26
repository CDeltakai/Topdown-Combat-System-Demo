using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RangedWeapon : MonoBehaviour
{

    [SerializeField] protected RangedWeaponSO weaponData;
    [field:SerializeField] public float FireRate{ get; private set; }

    public DamagePayload damagePayload;





    public abstract void OnFire();

    public virtual void Reload(){}





}
