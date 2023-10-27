using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AmmoCounter : MonoBehaviour
{
    [SerializeField] RangedWeapon currentWeapon;

    [SerializeField] TextMeshProUGUI magazineCount;
    [SerializeField] TextMeshProUGUI reserveCount;
    [SerializeField] TextMeshProUGUI seperator;


    void Start()
    {
        if(currentWeapon)
        {
            currentWeapon.OnDischarge += UpdateCounters;
            currentWeapon.OnReload += UpdateCounters;

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
        currentWeapon.OnReload -= UpdateCounters;

        currentWeapon = rangedWeapon;

        currentWeapon.OnDischarge += UpdateCounters;
        currentWeapon.OnReload += UpdateCounters;

        UpdateCounters();

    }


    void UpdateCounters()
    {
        if(currentWeapon.WeaponData.DrawsFromReserve)
        {
            magazineCount.text = currentWeapon.CurrentReserve.ToString();
            reserveCount.text = currentWeapon.ReserveCapacity.ToString();
        }else
        {
            magazineCount.text = currentWeapon.CurrentMagazine.ToString();
            reserveCount.text = currentWeapon.CurrentReserve.ToString();
        }


    }

}
