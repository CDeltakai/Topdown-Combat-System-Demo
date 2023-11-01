using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A generic metering system that can be used for measuring any number related events. Particularly useful for creating
/// GUI slider meters that need to measure some number value.
/// </summary>
public class ResourceMeter : MonoBehaviour
{
//Each value type has two events assosciated with it: OnXValueChanged and OnXValueModified. The difference between the two is just
//their input parameters. OnXValueChanged inputs an old value and new value and allows for more granular control and operations on the
//actual values. OnXValueModified on the other hand just raises whenever the value itself is modified some way and requires no parameters - 
//useful for when you only need to know when the value has changed and not the specifics of how it changed.

    public delegate void IntValueChangedEventHandler(int oldValue, int newValue);
    public event IntValueChangedEventHandler OnIntValueChanged;
    public delegate void IntValueModifiedEventHandler();
    public event IntValueModifiedEventHandler OnIntValueModified;


    public delegate void FloatValueChangedEventHandler(float oldValue, float newValue);
    public event FloatValueChangedEventHandler OnFloatValueChanged;
    public delegate void FloatValueModifiedEventHandler();
    public event FloatValueModifiedEventHandler OnFloatValueModified;


    public delegate void DoubleValueChangedEventHandler(double oldValue, double newValue);
    public event DoubleValueChangedEventHandler OnDoubleValueChanged;
    public delegate void DoubleValueModifiedEventHandler();
    public event DoubleValueModifiedEventHandler OnDoubleValueModified;


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
                OnIntValueModified?.Invoke();
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
                OnFloatValueModified?.Invoke();
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
                OnDoubleValueModified?.Invoke();
            }
        }
    }
    [SerializeField] double _maxDoubleValue;
    public double MaxDoubleValue{get { return _maxDoubleValue; }}
    public bool ignoreMaxDoubleValue = false;


    void Awake()
    {
        if(!ignoreMaxDoubleValue)
        {
            if(MaxDoubleValue < CurrentDoubleValue)
            {
                _maxDoubleValue = _currentDoubleValue;
            }
        }

        if(!ignoreMaxFloatValue)
        {
            if(MaxFloatValue < CurrentFloatValue)
            {
                _maxFloatValue = _currentFloatValue;
            }        
        }

        if(!ignoreMaxIntValue)
        {
            if(MaxIntValue < CurrentIntValue)
            {
                _maxIntValue = _currentIntValue;
            }
        }

    }


    public float GetIntValuePercentage()
    {
        if(ignoreMaxIntValue)
        {
            Debug.LogWarning("Max value has been ignored for the Int Value on this ResourceMeter, returned float.Epsilon for GetIntValuePercentage", this);
            return float.Epsilon;
        }
        return (float)_currentIntValue / _maxIntValue;
    }

    public float GetFloatValuePercentage()
    {
        if(ignoreMaxFloatValue)
        {
            Debug.LogWarning("Max value has been ignored for the Float Value on this ResourceMeter, returned float.Epsilon for GetFloatValuePercentage", this);
            return float.Epsilon;
        }        
        return _currentFloatValue / _maxFloatValue;
    }

    public float GetDoubleValuePercentage()
    {
        if(ignoreMaxDoubleValue)
        {
            Debug.LogWarning("Max value has been ignored for the Double Value on this ResourceMeter, returned float.Epsilon for GetDoubleValuePercentage", this);
            return float.Epsilon;
        }         
        return (float)(_currentDoubleValue / _maxDoubleValue);
    }



}
