using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A generic metering system that can be used for measuring any number related events. Particularly useful for creating
/// GUI slider meters that need to measure some number value.
/// </summary>
public class ResourceMeter : MonoBehaviour
{
    public delegate void IntValueChangedEventHandler(int oldValue, int newValue);
    public event IntValueChangedEventHandler OnIntValueChanged;

    public delegate void FloatValueChangedEventHandler(float oldValue, float newValue);
    public event FloatValueChangedEventHandler OnFloatValueChanged;

    public delegate void DoubleValueChangedEventHandler(double oldValue, double newValue);
    public event DoubleValueChangedEventHandler OnDoubleValueChanged;

[Header("Int Values")]
    [Min(0)]
    [SerializeField] int _currentIntValue;
    public int CurrentIntValue 
    {
        get{return _currentIntValue;}

        set
        {
            if(_currentIntValue != value)
            {
                int oldValue = _currentIntValue;

                if(_currentIntValue + value > _maxIntValue && !ignoreMaxIntValue)
                {
                    _currentIntValue = _maxIntValue;
                }else
                if(_currentIntValue + value <= 0)
                {
                    _currentIntValue = 0;
                }else
                {
                    _currentIntValue = value;
                }

                OnIntValueChanged?.Invoke(oldValue, _currentIntValue);
            }
        }
    }
    [SerializeField] int _maxIntValue;
    public int MaxIntValue{get { return _maxIntValue; }}
    public bool ignoreMaxIntValue = false;


[Header("Float Values")]
    [Min(0)]
    [SerializeField] float _currentFloatValue;
    public float CurrentFloatValue 
    {
        get{return _currentFloatValue;}

        set
        {
            if(_currentFloatValue != value)
            {
                float oldValue = _currentFloatValue;

                if(_currentFloatValue + value > _maxFloatValue && !ignoreMaxFloatValue)
                {
                    _currentFloatValue = _maxFloatValue;
                }else
                if(_currentFloatValue + value <= 0)
                {
                    _currentFloatValue = 0;
                }else
                {
                    _currentFloatValue = value;
                }

                OnFloatValueChanged?.Invoke(oldValue, _currentFloatValue);
            }
        }
    }
    [SerializeField] float _maxFloatValue;
    public float MaxFloatValue{get { return _maxFloatValue; }}
    public bool ignoreMaxFloatValue = false;



[Header("Double Values")]
    [Min(0)]
    [SerializeField] double _currentDoubleValue;
    public double CurrentDoubleValue 
    {
        get{return _currentDoubleValue;}

        set
        {
            if(_currentDoubleValue != value)
            {
                double oldValue = _currentDoubleValue;
                if(_currentDoubleValue + value > _maxDoubleValue && !ignoreMaxDoubleValue)
                {
                    _currentDoubleValue = _maxDoubleValue;
                }else
                if(_currentDoubleValue + value <= 0)
                {
                    _currentDoubleValue = 0;
                }else
                {
                    _currentDoubleValue = value;
                }
                OnDoubleValueChanged?.Invoke(oldValue, _currentDoubleValue);
            }
        }
    }
    [SerializeField] double _maxDoubleValue;
    public double MaxDoubleValue{get { return _maxDoubleValue; }}
    public bool ignoreMaxDoubleValue = false;


    void Awake()
    {
        if(MaxDoubleValue < CurrentDoubleValue)
        {
            _maxDoubleValue = _currentDoubleValue;
        }

        if(MaxFloatValue < CurrentFloatValue)
        {
            _maxFloatValue = _currentFloatValue;
        }        

        if(MaxIntValue < CurrentIntValue)
        {
            _maxIntValue = _currentIntValue;
        }

    }


    public float GetIntValuePercentage()
    {
        return (float)_currentIntValue / _maxIntValue;
    }

    public float GetFloatValuePercentage()
    {
        return _currentFloatValue / _maxFloatValue;
    }

    public float GetDoubleValuePercentage()
    {
        return (float)(_currentDoubleValue / _maxDoubleValue);
    }



}
