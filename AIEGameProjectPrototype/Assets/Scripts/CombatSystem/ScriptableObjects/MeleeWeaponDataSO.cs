using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "Melee Weapon Data", menuName = "New Melee Weapon Data", order = 0)]
public class MeleeWeaponDataSO : ScriptableObject
{
[Header("Combat Attributes")]
    [SerializeField] string _weaponName;
    public string WeaponName {get{ return _weaponName; }}

    [field:SerializeField] public DamagePayload Payload{ get; private set; }

    [Range(0, 10)]
    [SerializeField] float _swingCooldown;
    public float SwingCooldown{ get { return _swingCooldown; } }


[Tooltip("The modifier for how much bigger the hitbox collider for the melee weapon is.")]
    [Min(0)]
    [SerializeField] Vector2 _attackRangeMod;
    public Vector2 AttackRangeMod{get{ return _attackRangeMod; }}


}
