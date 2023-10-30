using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A weapon management system that allows usage of any number of weapons with selection capabilities.
/// </summary>
public class WeaponHolder : MonoBehaviour
{
    public delegate void ScrollWeaponEventHandler(RangedWeapon weapon);
    public event ScrollWeaponEventHandler OnScrollWeapon;

    [SerializeField] List<GameObject> weaponsToSpawn = new List<GameObject>();
    [SerializeField] List<RangedWeapon> weaponsInScene = new List<RangedWeapon>();

    [SerializeField] RangedWeapon _currentWeapon;
    public RangedWeapon CurrentWeapon {get{ return _currentWeapon; }}

    int currentWeaponIndex;

    void Awake()
    {
        foreach(var weaponPrefab in weaponsToSpawn)
        {
            if(weaponPrefab.GetComponent<RangedWeapon>() != null)
            {
                RangedWeapon weaponInstance = Instantiate(weaponPrefab, transform).GetComponent<RangedWeapon>();
                weaponInstance.InitializeProjectilePool();
                weaponPrefab.SetActive(false);
                weaponsInScene.Add(weaponInstance);
            }else
            {
                Debug.LogWarning("A prefab: " + weaponPrefab.name + " does not have a RangedWeapon component attached, skipped instantiation.");
                continue;
            }
        }

        if(weaponsInScene.Count > 0)
        {
            _currentWeapon = weaponsInScene[0];
            _currentWeapon.gameObject.SetActive(true);
            currentWeaponIndex = 0;
        }
    }



    public void ScrollWeaponForward()
    {
        if(weaponsInScene.Count <= 1) { return; }

        currentWeaponIndex++;

        if(currentWeaponIndex >= weaponsInScene.Count)
        {
            currentWeaponIndex = 0;

            _currentWeapon.StopOperations();

            _currentWeapon.gameObject.SetActive(false);
            _currentWeapon = weaponsInScene[currentWeaponIndex];
            _currentWeapon.gameObject.SetActive(true);

            OnScrollWeapon?.Invoke(_currentWeapon);

        }else
        {
            _currentWeapon.StopOperations();

            _currentWeapon.gameObject.SetActive(false);
            _currentWeapon = weaponsInScene[currentWeaponIndex];
            _currentWeapon.gameObject.SetActive(true);

            OnScrollWeapon?.Invoke(_currentWeapon);
        }

    }

    public void ScrollWeaponBackwards()
    {
        if(weaponsInScene.Count <= 1) { return; }

        currentWeaponIndex--;

        if(currentWeaponIndex == 0)
        {
            currentWeaponIndex = weaponsInScene.Count - 1;

            _currentWeapon.StopOperations();

            _currentWeapon.gameObject.SetActive(false);
            _currentWeapon = weaponsInScene[currentWeaponIndex];
            _currentWeapon.gameObject.SetActive(true);

            OnScrollWeapon?.Invoke(_currentWeapon);

        }else
        {
            _currentWeapon.StopOperations();

            _currentWeapon.gameObject.SetActive(false);
            _currentWeapon = weaponsInScene[currentWeaponIndex];
            _currentWeapon.gameObject.SetActive(true);

            OnScrollWeapon?.Invoke(_currentWeapon);
        }

    }


}
