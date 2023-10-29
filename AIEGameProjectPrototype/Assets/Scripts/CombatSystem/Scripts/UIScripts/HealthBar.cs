using System.Runtime.InteropServices;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class HealthBar : MonoBehaviour
{

    [SerializeField] HealthMeter meter;
    [SerializeField] Slider bar;

    public bool beginVisible;

    void Awake()
    {
        if(beginVisible)
        {
            bar.gameObject.SetActive(true);
        }else
        {
            bar.gameObject.SetActive(false);
        }

        meter.OnHPChanged += UpdateBar;

    }

    private void UpdateBar(int oldValue, int newValue)
    {
        float percentRemaining = (float) meter.CurrentHP / meter.MaxHP;
        bar.value = percentRemaining;
        bar.gameObject.SetActive(true);
    }




}
