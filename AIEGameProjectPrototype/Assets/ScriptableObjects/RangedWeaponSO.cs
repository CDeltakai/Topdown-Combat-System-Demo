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
public class RangedWeaponSO : ScriptableObject
{

[Header("Combat Attributes")]
    [SerializeField] string weaponName;

    [field:SerializeField] public DamagePayload Payload{ get; private set; }

    [Range(0, 10)]
    [SerializeField] float _fireRate;
    public float FireRate{ get { return _fireRate; } }

    [field:SerializeField] public bool FullAuto { get; private set; }

[Tooltip("The number of shots the weapon fires on a single trigger pull.")]
    [Range(1, 50)]
    [SerializeField] int _burstCount;
    public int BurstCount { get{ return _burstCount; } }

    [Min(0)]
    [SerializeField] int _magazineCapacity;
    public int MagazineCapacity { get { return _magazineCapacity; }}

    [Min(0)]
    [SerializeField] int _reserveCapacity;
    public int ReserveCapacity { get { return _magazineCapacity; }}


[Tooltip("The random angular deviation of the weapon's projectiles when fired.")]
    [Range(0, 360)]
    [SerializeField] float _spread;
    public float Spread { get{ return _spread; } }

[Header("Advanced Properties")]
    [Tooltip("The projectile prefab the weapon will fire.")]
    [SerializeField] GameObject _projectilePrefab;
    public GameObject ProjectilePrefab { get{ return _projectilePrefab; } }

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
