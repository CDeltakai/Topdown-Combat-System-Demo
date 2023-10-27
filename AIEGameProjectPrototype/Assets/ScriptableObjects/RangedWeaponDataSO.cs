using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DamageType
{
    Physical,
    Piercing,
    Fire
}


[CreateAssetMenu(fileName = "Ranged Weapon Data", menuName = "New Ranged Weapon Data", order = 0)]
public class RangedWeaponDataSO : ScriptableObject
{
    public const float MINIMUM_FIRERATE = 0.05f;


[Header("Combat Attributes")]
    [SerializeField] string _weaponName;
    public string WeaponName {get{ return _weaponName; }}

    [field:SerializeField] public DamagePayload Payload{ get; private set; }

[Tooltip("The time delay in seconds before another round can be fired. Higher = slower fire-rate. 0 means the player can fire the weapon as quickly as they click the mouse." +
" If the weapon is full auto, the minimum fire-rate will be set to 0.05f.")]
    [Range(0, 10)]
    [SerializeField] float _fireRate;
    public float FireRate{ get { 
        if(FullAuto && _fireRate < MINIMUM_FIRERATE)
        {
            return MINIMUM_FIRERATE;
        }
        return _fireRate; } }

    [field:SerializeField] public bool FullAuto { get; private set; }

[Tooltip("The number of shots the weapon fires on a single trigger pull.")]
    [Range(1, 50)]
    [SerializeField] int _burstCount = 1;
    public int BurstCount { get{ return _burstCount; } }

    [Min(0)]
    [SerializeField] int _magazineCapacity;
    public int MagazineCapacity { get { return _magazineCapacity; }}


[Tooltip("How long it takes for the weapon to reload in seconds.")]
    [Min(0)]
    [SerializeField] float _reloadDuration;
    public float ReloadDuration { get { return _reloadDuration; }}


[Tooltip("Dictates whether the weapon draws munitions directly from its ammo reserve, ignoring magazine capacity. Also disables reloading if set to true.")]
    [SerializeField] bool _drawsFromReserve;
    public bool DrawsFromReserve { get { return _drawsFromReserve; }}


    [Min(0)]
    [SerializeField] int _reserveCapacity;
    public int ReserveCapacity { get { return _reserveCapacity; }}


[Tooltip("The random angular deviation of the weapon's projectiles when fired.")]
    [Range(0, 360)]
    [SerializeField] float _spread;
    public float Spread { get{ return _spread; } }

[Header("Advanced Properties")]
    [Tooltip("The projectile prefab the weapon will fire.")]
    [SerializeField] GameObject _projectilePrefab;
    public GameObject ProjectilePrefab { get{ return _projectilePrefab; } }

    [Range(0.01f, 200)]
    [Tooltip("How long the projectile will stay in the scene before self-destructing.")]
    [SerializeField] float _projectileLifetime;
    public float ProjectileLifetime{get { return _projectileLifetime; }}

    [Tooltip("The muzzle flash particle effect that triggers when this weapon is fired.")]
    [SerializeField] GameObject _muzzleFlashPrefab;
    public GameObject MuzzleFlashPrefab { get{ return _muzzleFlashPrefab; } }

[Header("Miscellaneous Properties")]
    [Tooltip("How long the camera shakes for when the weapon is fired.")]
    [Min(0)]
    [SerializeField] float _cameraShakeDuration;
    public float CameraShakeDuration { get { return _cameraShakeDuration; }}

    [Tooltip("The strength of camera shake when this weapon is fired.")]
    [Min(0)]
    [SerializeField] float _cameraShakeMagnitude;
    public float CameraShakeMagnitude { get { return _cameraShakeMagnitude; }}    


}
