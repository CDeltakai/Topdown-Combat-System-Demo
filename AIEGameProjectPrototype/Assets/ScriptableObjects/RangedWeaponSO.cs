using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    [Range(1, 50)]
    [SerializeField] int _burstCount;
    public int BurstCount { get{ return _burstCount; } }


    [field:SerializeField] public int MagazineCapacity { get; private set; }
    [field:SerializeField] public int ReserveCapacity { get; private set; }

    [Range(0, 360)]
    [SerializeField] float _spread;
    public float Spread { get{ return _spread; } }

[Header("Advanced Properties")]
    [SerializeField] GameObject _projectilePrefab;
    public GameObject ProjectilePrefab { get{ return _projectilePrefab; } }




}
