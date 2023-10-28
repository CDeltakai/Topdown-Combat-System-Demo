using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AmmoCounterWorld : MonoBehaviour
{
    public WeaponHolder weaponHolder;
    [SerializeField] RangedWeapon currentWeapon;

    [SerializeField] TextMeshPro magazineCount;
    [SerializeField] TextMeshPro reserveCount;


    void Start()
    {
        InitializeWeaponHolder();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitializeWeaponHolder()
    {
        if(weaponHolder)
        {
            currentWeapon = weaponHolder.CurrentWeapon;

            currentWeapon.OnDischarge += UpdateCounters;
            currentWeapon.OnFinishReload += UpdateCounters;

            weaponHolder.OnScrollWeapon += ChangeWeapon;

            UpdateCounters();            
        }else
        {
            Debug.LogWarning("No weapon holder set, ammo counter will not function.");
        }        
    }

    public void ChangeWeapon(RangedWeapon rangedWeapon)
    {
        currentWeapon.OnDischarge -= UpdateCounters;
        currentWeapon.OnFinishReload -= UpdateCounters;

        currentWeapon = rangedWeapon;

        currentWeapon.OnDischarge += UpdateCounters;
        currentWeapon.OnFinishReload += UpdateCounters;

        UpdateCounters();

    }


    void UpdateCounters()
    {
        if(currentWeapon.WeaponData.DrawsFromReserve)
        {
            if(magazineCount) { magazineCount.text = currentWeapon.CurrentReserve.ToString(); }
            if(reserveCount) { reserveCount.text = currentWeapon.ReserveCapacity.ToString(); }
        }else
        {
            if(magazineCount) {magazineCount.text = currentWeapon.CurrentMagazine.ToString();}
            if(reserveCount) {reserveCount.text = currentWeapon.CurrentReserve.ToString();}
        }


    }
}
