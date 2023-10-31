using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A flexible and modular health and damage meter + system that can be attached to any object and referenced in order to track
/// the HP of some object.
/// </summary>
public class HealthMeter : MonoBehaviour
{
    public delegate void ShieldHPChangedHandler(int oldValue, int newValue);
    public event ShieldHPChangedHandler OnShieldHPChanged;

    public delegate void CurrentHPChangedHandler(int oldValue, int newValue);
    public event CurrentHPChangedHandler OnHPChanged;

    public delegate void DamageTakenEventHandler(int damageTaken);
    public event DamageTakenEventHandler OnDamageTaken;

    public delegate void HPDepletedEventHandler();
    public event HPDepletedEventHandler OnHPDepleted;

    public delegate void HPFullEventHandler();
    public event HPFullEventHandler OnHPFull;

[Tooltip("Whether this health meter can take damage.")]
    public bool canTakeDamage = true;

[Tooltip("Whether this health meter will self-disable after being depleted.")]
    public bool disableAfterDepletion = false;

[Tooltip("If false, object will not react to method calls.")]
    public bool isActive = true;

    [Min(0)]
    [SerializeField] int _currentHP = 100;
    //Publically accessible property for currentHP that when set, will set the value of the private currentHP value and raise the OnHPChanged event.
    public int CurrentHP {
        get{return _currentHP;}
        private set
        {
            if(_currentHP != value)
            {
                int oldValue = _currentHP;
                _currentHP = value;

                OnHPChanged?.Invoke(oldValue, _currentHP);
            }
        }

    }


[Tooltip("The cap that Current HP can be increased to. If Max HP is set lower than Current HP, it will automatically be set to be Current HP.")]
    [Min(0)]
    [SerializeField] int _maxHP;
    public int MaxHP {get { return _maxHP; }}


    [Min(0)]
    [SerializeField] int _shieldHP;
    //Publically accessible property for shieldHP that when set, will set the value of the private shieldHP value and raise the OnShieldHPChanged event.
    public int ShieldHP {
        get{return _shieldHP;}
        private set
        {
            if(_shieldHP != value)
            {
                int oldValue = _shieldHP;
                _shieldHP = value;
                OnShieldHPChanged?.Invoke(oldValue, _shieldHP);
            }
        }
    }

[Tooltip("The percentage value which an incoming attack damage will be multiplied by. Unlike defense, armor cannot increase damage dealt, only reduce it.")]
    [Range(0, 100)]
    public int armor = 0;

[Tooltip("The multiplier for incoming damage. Values below 1 will reduce incoming damage whilst values above it will increase damage.")]
    [Range(0.1f, 10f)]
    public double defenseMultiplier = 1;


    void Awake()
    {
        if(_maxHP < _currentHP)
        {
            _maxHP = _currentHP;
        }

        if(disableAfterDepletion)
        {
            OnHPDepleted += SelfDisable;
        }

    }


/// <summary>
/// Deal damage to the HealthMeter. If the HealthMeter has ShieldHP, the ShieldHP takes damage before the CurrentHP.
/// </summary>
/// <param name="payload"></param>
    public virtual void Hurt(DamagePayload payload)
    {
        if(!isActive){ return; }
        if(!canTakeDamage){return;}

        DamagePayload finalPayload = payload;
        finalPayload.baseDamage = Math.Abs(payload.baseDamage);

        int damageAfterModifiers;
        int originalShieldHP = ShieldHP;
        bool wentThroughShields = false;

        //Calculate damage for Shields
        ShieldHP -= finalPayload.baseDamage;
        if(_shieldHP < 0)
        {
            damageAfterModifiers = Math.Abs(_shieldHP);
            _shieldHP = 0;
            if(originalShieldHP > 0)
            {
                wentThroughShields = true;
            }
        }else
        {
            damageAfterModifiers = 0;
            OnDamageTaken?.Invoke(finalPayload.baseDamage);   
        }

        //Damage calculations
        if(damageAfterModifiers != 0)
        {
            damageAfterModifiers = (int)(damageAfterModifiers * ((100 - armor) * 0.01) * defenseMultiplier);
            if(wentThroughShields)
            {
                OnDamageTaken?.Invoke(originalShieldHP + damageAfterModifiers);
            }else
            {
                OnDamageTaken?.Invoke(damageAfterModifiers);
            }
        }
        
        CurrentHP -= damageAfterModifiers;
        if(_currentHP <= 0)
        {
            OnHPDepleted?.Invoke();
            _currentHP = 0;
        }

    }
    public void Hurt(int damage)
    {
        if(!isActive){ return; }
        if(!canTakeDamage){return;}        

        DamagePayload tempPayload = new DamagePayload()
        {
            baseDamage = damage
        };
        Hurt(tempPayload);
    }


    void SelfDisable()
    {
        if(disableAfterDepletion)
        {
            isActive = false;
        }
    }


/// <summary>
/// Restore a given amount of HP to CurrentHP. Cannot restore above MaxHP. If given 0 or no value, will restore meter to full.
/// </summary>
/// <param name="amount"></param>
    public void RestoreHP(int amount = 0)
    {
        if(!isActive){ return; }        

        if(amount == 0)
        {
            _currentHP = _maxHP;
            OnHPFull?.Invoke();
            return;
        }

        if(_currentHP + amount >= _maxHP)
        {
            CurrentHP = _maxHP;
            OnHPFull?.Invoke();
        }else
        {
            CurrentHP += amount;
        }
    }

/// <summary>
/// Sets the max HP of the meter. If given value is less than current HP but not less than or equal to 0, will set max HP to current HP.
/// If given value is less than or equal to 0, will do nothing.
/// </summary>
/// <param name="amount"></param>
    public void SetMaxHP(int amount)
    {
        if(!isActive){ return; }        

        if(amount <= 0)
        {
            return;
        }

        if(amount < _currentHP)
        {
            _maxHP = _currentHP;
        }else
        {
            _maxHP = amount;
        }
    }

    public float GetPercentageRemaining()
    {
        return (float)_currentHP / _maxHP;
    }


}
