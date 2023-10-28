using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHolder : MonoBehaviour
{
    [SerializeField] List<RangedWeapon> weaponsToSpawn = new List<RangedWeapon>();

    [SerializeField] List<RangedWeapon> weaponsInScene = new List<RangedWeapon>();
    [SerializeField] RangedWeapon currentWeapon;




    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
