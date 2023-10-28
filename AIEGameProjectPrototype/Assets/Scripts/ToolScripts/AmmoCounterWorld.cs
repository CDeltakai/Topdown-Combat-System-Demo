using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AmmoCounterWorld : MonoBehaviour
{
    [SerializeField] RangedWeapon currentWeapon;

    [SerializeField] TextMeshPro magazineCount;
    [SerializeField] TextMeshPro reserveCount;


    void Start()
    {
        if(currentWeapon)
        {
            currentWeapon.OnDischarge += UpdateCounters;
            currentWeapon.OnFinishReload += UpdateCounters;

            UpdateCounters();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
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
