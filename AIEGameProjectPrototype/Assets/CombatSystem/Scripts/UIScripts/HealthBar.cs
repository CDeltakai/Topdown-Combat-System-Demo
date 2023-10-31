using System.Runtime.InteropServices;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

/// <summary>
/// Simple UI health bar component which can be hooked up to any HealthMeter to track its HP.
/// </summary>
public class HealthBar : MonoBehaviour
{

    public HealthMeter meter;
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
        bar.value = meter.GetPercentageRemaining();
        bar.gameObject.SetActive(true);
    }




}
