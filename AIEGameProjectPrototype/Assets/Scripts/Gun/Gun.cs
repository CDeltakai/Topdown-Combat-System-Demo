using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] GameObject projectile;
    [SerializeField] Transform firePoint;

    [SerializeField] int damage = 1;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void OnFire()
    {
        Bullet bullet = Instantiate(projectile, firePoint.position, firePoint.rotation).GetComponent<Bullet>();
        bullet.rigBody.velocity = firePoint.forward * bullet.speed;

    }





}
