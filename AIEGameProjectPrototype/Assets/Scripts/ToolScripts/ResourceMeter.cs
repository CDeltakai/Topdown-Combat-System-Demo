using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceMeter : MonoBehaviour
{
    public delegate void ValueChangedEventHandler(int oldValue, int newValue);
    public event ValueChangedEventHandler OnIntValueChanged;

    public delegate void FloatValueChangedEventHandler(float oldValue, float newValue);
    public event FloatValueChangedEventHandler OnFloatValueChanged;


    [SerializeField] int _currentIntValue;
    public int CurrentIntValue 
    {
        get{return _currentIntValue;}

        set
        {
            if(_currentIntValue != value)
            {
                int oldValue = _currentIntValue;
                _currentIntValue = value;
                OnIntValueChanged?.Invoke(oldValue, _currentIntValue);
            }
        }
    }

    [SerializeField] float _currentFloatValue;
    public float CurrentFloatValue 
    {
        get{return _currentFloatValue;}

        set
        {
            if(_currentFloatValue != value)
            {
                float oldValue = _currentFloatValue;
                _currentFloatValue = value;
                OnFloatValueChanged?.Invoke(oldValue, _currentFloatValue);
            }
        }
    }


    [SerializeField] int _maxIntValue;
    public int MaxValue{get { return _maxIntValue; }}

    [SerializeField] float _maxFloatValue;
    public float MaxFloatValue{get { return _maxFloatValue; }}



    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
