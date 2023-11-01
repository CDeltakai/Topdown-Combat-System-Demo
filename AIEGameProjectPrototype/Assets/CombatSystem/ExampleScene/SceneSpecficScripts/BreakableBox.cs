using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableBox : MonoBehaviour
{
    [SerializeField] HealthMeter health;

    void Start()
    {
        health.OnHPDepleted += SelfDestruct;
    }


    void SelfDestruct()
    {
        Destroy(gameObject);
    }

}
