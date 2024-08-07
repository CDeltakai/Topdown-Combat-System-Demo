using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// Base class that handles generic functionality of a ranged weapon.
/// </summary>
[RequireComponent(typeof(AudioSource))]
public abstract class RangedWeapon : MonoBehaviour
{
#region Events and Delegates
    public delegate void DischargeWeaponEventHandler();
    public event DischargeWeaponEventHandler OnDischarge;

    public delegate void FinishReloadWeaponEventHandler();
    public event FinishReloadWeaponEventHandler OnFinishReload;

    public delegate void StartReloadWeaponEventHandler();
    public event StartReloadWeaponEventHandler OnStartReload;

    public delegate void AmmoChangedEventHandler();
    public event AmmoChangedEventHandler OnAmmoChanged;

#endregion

[Tooltip("The point where projectiles will be spawned from.")]
    [SerializeField] protected Transform firePoint;
    [SerializeField] protected RangedWeaponDataSO _weaponData;
    public RangedWeaponDataSO WeaponData {get{ return _weaponData; }}

    protected ParticleSystem muzzleFlash;
    protected DamagePayload damagePayload;
    protected CinemachineImpulseSource cinemachineImpulseSource;
    protected GameObjectPool projectilePool;
    protected AudioSource audioSource;


    [SerializeField] protected int _magazineCapacity;
    public int MagazineCapacity{get{ return _magazineCapacity; }}

    [SerializeField] protected int _currentMagazine;
    public int CurrentMagazine{get{ return _currentMagazine; }
        private set
        {
            if(_currentMagazine != value)
            {
                _currentMagazine = value;
                OnAmmoChanged?.Invoke();
            }
        }
    }

    [SerializeField] protected int _reserveCapacity;
    public int ReserveCapacity{get{ return _reserveCapacity; }}

    [SerializeField] protected int _currentReserve;
    public int CurrentReserve{get{ return _currentReserve; }
        private set
        {
            if(_currentReserve != value)
            {
                _currentReserve = value;
                OnAmmoChanged?.Invoke();
            }
        }
    }

[Tooltip("If set to true, the weapon will be able to fire regardless of ammo and will not consume ammo.")]
    public bool infiniteAmmo = false;

[Tooltip("If set to true, weapon will use experimental FixedCooldown system to calculate weapon fire-rate. Theoretically should maintain" + 
" more consistent and accurate fire-rate even at lower framerates.")]
    [SerializeField] bool UseFixedCooldown;

    float timeSinceLastShot { get; set; }

    bool canFire = true;
    bool hasFired = false;
    Coroutine CR_ReloadTimer = null;
    Coroutine CR_Cooldown = null;



    protected virtual void Awake() 
    {
        damagePayload = _weaponData.Payload;

        _magazineCapacity = _weaponData.MagazineCapacity;
        _currentMagazine = _magazineCapacity;

        _reserveCapacity = _weaponData.ReserveCapacity;
        _currentReserve = _reserveCapacity;

        if(_weaponData.MuzzleFlashPrefab)
        {
            muzzleFlash = Instantiate(_weaponData.MuzzleFlashPrefab, firePoint).GetComponent<ParticleSystem>();
        }

        cinemachineImpulseSource = GetComponent<CinemachineImpulseSource>();
        audioSource = GetComponent<AudioSource>();

        InitializeProjectilePool();
    }

    protected virtual void Start(){}

    void FixedUpdate()
    {
        if(UseFixedCooldown)
        {
            FixedCooldown();
        }
    }

/// <summary>
/// Experimental fixed update based weapon cooldown system - objective is to hopefully reduce the impact of framerate
/// on weapon fire-rate when framerate is low.
/// </summary>
    void FixedCooldown()
    {
        if(hasFired)
        {
            timeSinceLastShot += Time.fixedDeltaTime;
            if(timeSinceLastShot >= WeaponData.FireRate)
            {
                canFire = true;
                hasFired = false;
            }else
            {
                canFire = false;
            }
        }
    }


    /// <summary>
    /// Dynamically create a new projectile pool in the scene for optimization purposes
    /// </summary>
    public void InitializeProjectilePool()
    {
        if(projectilePool != null) { return; } // can only have one projectile pool per gun

        GameObject newProjectilePool = new(_weaponData.WeaponName +"_ProjectilePool");
        newProjectilePool.AddComponent<GameObjectPool>();
        projectilePool = newProjectilePool.GetComponent<GameObjectPool>();
        projectilePool.prefab = _weaponData.ProjectilePrefab;

        //Add some starting projectiles to the pool to smoothen gameplay
        //If the weapon is full auto, it will add objects according to the weapon's fire rate and burst count
        //to match the rate at which objects will be used.
        if(!_weaponData.FullAuto)
        {
            //10 is the average Clicks Per Second for most players, thus is a good approximate for how many objects to add at the start.
            projectilePool.AddObject(10 * _weaponData.BurstCount); 
        }else
        {
            projectilePool.AddObject((int)( 1 / _weaponData.FireRate * _weaponData.BurstCount));
        }


    }

    public virtual void PullTrigger()
    {
        if(!canFire){ return; }
        if(!infiniteAmmo)
        {
            if(!CheckAmmo())
            {
                if(_weaponData.DrawsFromReserve)
                {
                    return; 
                }
                StartReload();
                return;
            }
        }

        for(int i = 0; i < _weaponData.BurstCount; i++) 
        {
            InitializeProjectile();           
        }

        //Decrease ammo count
        if(!infiniteAmmo)
        {
            if(_weaponData.DrawsFromReserve)
            {
                CurrentReserve--;
            }else
            {
                CurrentMagazine--;
            }
        }

        //Play muzzle flash if the weapon has one
        if(muzzleFlash){muzzleFlash.Play();}
        if(WeaponData.OnFireSFX){audioSource.PlayOneShot(WeaponData.OnFireSFX);}

        //Cooldown
        if(UseFixedCooldown)
        {
            hasFired = true;
            canFire = false;
            timeSinceLastShot = 0f;
        }else
        {
            CR_Cooldown = StartCoroutine(Cooldown(_weaponData.FireRate));
        }

        ShakeCamera();
        OnDischarge?.Invoke();
    }


    protected virtual void InitializeProjectile()
    {
        float spread = Random.Range(-_weaponData.Spread, _weaponData.Spread);

        Vector3 fireDirection = firePoint.forward;

        // Rotate the fire direction around the global up axis by the spread angle
        fireDirection = Quaternion.AngleAxis(spread, Vector3.up) * fireDirection;

        GameObject bulletObject = projectilePool.UseObject(bulletObject =>
        {
            Bullet bullet = bulletObject.GetComponent<Bullet>();
            bullet.objectIsPooled = true;
            bullet.transform.position = firePoint.position;
            bullet.rigBody.velocity = fireDirection * bullet.speed;
            bullet.lifetime = _weaponData.ProjectileLifetime;
            bullet.damagePayload = damagePayload;        
        });

    }


    public void StartReload()
    {
        if(_weaponData.DrawsFromReserve) { return; }
        if(CR_ReloadTimer != null) { return; } // if gun is already reloading, return
        if(_currentMagazine == _magazineCapacity) { return; } // if gun is already full, return
        if(_currentReserve <= 0 ) { return; }

        if(WeaponData.OnStartReloadSFX){ audioSource.PlayOneShot(WeaponData.OnStartReloadSFX); }

        OnStartReload?.Invoke();
        CR_ReloadTimer = StartCoroutine(ReloadTimer(_weaponData.ReloadDuration));
    }

    void Reload()
    {
        int requestedAmmo = _magazineCapacity - _currentMagazine;

        if(requestedAmmo > _currentReserve)
        {
            CurrentMagazine += CurrentReserve;
            CurrentReserve = 0;
        }else
        {
            CurrentMagazine = _magazineCapacity;
            CurrentReserve -= requestedAmmo;
        }

        if(WeaponData.OnFinishReloadSFX){ audioSource.PlayOneShot(WeaponData.OnFinishReloadSFX); }
        OnFinishReload?.Invoke();

    }

    protected IEnumerator ReloadTimer(float duration = 1)
    {
        if(CR_Cooldown != null)
        {
            StopCoroutine(CR_Cooldown);
        }
        canFire = false;

        yield return new WaitForSeconds(duration * 0.5f);
        if(WeaponData.MidReloadSFX){ audioSource.PlayOneShot(WeaponData.MidReloadSFX); }
        yield return new WaitForSeconds(duration * 0.5f);

        Reload();
        CR_ReloadTimer = null;

        canFire = true;

    }

    public void RestoreAmmoToReserve(int amount)
    {
        CurrentReserve += amount;
        if(_currentReserve > _reserveCapacity)
        {
            CurrentReserve = _reserveCapacity;
        }
        OnDischarge?.Invoke();
    }

    //Returns true if there is still ammo left, false otherwise.
    protected bool CheckAmmo()
    {
        if(_weaponData.DrawsFromReserve)
        {
            if(_currentReserve <= 0)
            {
                return false;
            }
        }else
        {
            if(_currentMagazine <= 0)
            {
                return false;
            }
        }

        return true;
    }


    protected void ShakeCamera()
    {
        if(cinemachineImpulseSource)
        {
            cinemachineImpulseSource.m_DefaultVelocity = new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), 0);

            cinemachineImpulseSource.m_ImpulseDefinition.m_ImpulseDuration = _weaponData.CameraShakeDuration;
            cinemachineImpulseSource.GenerateImpulseWithForce(_weaponData.CameraShakeMagnitude);
        }
    }


    protected IEnumerator Cooldown(float duration)
    {
        canFire = false;
        yield return new WaitForSeconds(duration);
        canFire = true;
        CR_Cooldown = null;
    }


    public void StopOperations()
    {
        StopAllCoroutines();
        CR_Cooldown = null;
        CR_ReloadTimer = null;
        
        canFire = true;

    }

}
