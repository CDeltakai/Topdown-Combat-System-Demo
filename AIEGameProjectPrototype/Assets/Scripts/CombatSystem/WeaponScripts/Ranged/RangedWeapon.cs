using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using Random = UnityEngine.Random;

public abstract class RangedWeapon : MonoBehaviour
{
    public delegate void DischargeWeaponEventHandler();
    public event DischargeWeaponEventHandler OnDischarge;

    public delegate void FinishReloadWeaponEventHandler();
    public event FinishReloadWeaponEventHandler OnFinishReload;

    public delegate void StartReloadWeaponEventHandler();
    public event StartReloadWeaponEventHandler OnStartReload;


    [SerializeField] protected Transform firePoint;
    [SerializeField] protected RangedWeaponDataSO _weaponData;
    public RangedWeaponDataSO WeaponData {get{ return _weaponData; }}

    [SerializeField] ParticleSystem muzzleFlash;
    protected DamagePayload damagePayload;
    protected CinemachineImpulseSource cinemachineImpulseSource;


    [SerializeField] protected int _magazineCapacity;
    public int MagazineCapacity{get{ return _magazineCapacity; }}

    [SerializeField] protected int _currentMagazine;
    public int CurrentMagazine{get{ return _currentMagazine; }}

    [SerializeField] protected int _reserveCapacity;
    public int ReserveCapacity{get{ return _reserveCapacity; }}

    [SerializeField] protected int _currentReserve;
    public int CurrentReserve{get{ return _currentReserve; }}

    GameObjectPool projectilePool;

    bool canFire = true;
    public bool infiniteAmmo = false;

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

    }

    protected virtual void Start()
    {
        CreateProjectilePool();
    }

    //Dynamically create a new projectile pool in the scene for optimization purposes
    void CreateProjectilePool()
    {
        GameObject newProjectilePool = new(_weaponData.WeaponName +"_ProjectilePool");
        newProjectilePool.AddComponent<GameObjectPool>();
        projectilePool = newProjectilePool.GetComponent<GameObjectPool>();
        projectilePool.prefab = _weaponData.ProjectilePrefab;

        //Add some starting projectiles to the pool to smoothen gameplay
        //If the weapon is full auto, it will add objects according to the weapon's fire rate and burst count
        //to match the rate at which objects will be used.
        if(!_weaponData.FullAuto)
        {
            //10 is the average Clicks per Second for most players, thus is a good approximate for how many objects to add at the start
            projectilePool.AddObject(10 * _weaponData.BurstCount); 
        }else
        {
            projectilePool.AddObject((int)( 1 / _weaponData.FireRate * _weaponData.BurstCount));
        }


    }

    public virtual void PullTrigger()
    {
        if(!infiniteAmmo)
        {
            if(!canFire){ return; }
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
                _currentReserve--;
            }else
            {
                _currentMagazine--;
            }
        }

        //Play muzzle flash if the weapon has one
        if(muzzleFlash){muzzleFlash.Play();}

        //Cooldown
        CR_Cooldown = StartCoroutine(Cooldown(_weaponData.FireRate));

        ShakeCamera();

        OnDischarge?.Invoke();

    }

    void InitializeProjectile()
    {
        float spread = Random.Range(-_weaponData.Spread, _weaponData.Spread);
        Vector3 fireDirection = Quaternion.Euler(0, spread, 0) * firePoint.forward;

        GameObject bulletObject = projectilePool.UseObject(bulletObject =>
        {
            Bullet bullet = bulletObject.GetComponent<Bullet>();
            bullet.objectIsPooled = true;
            bullet.transform.SetPositionAndRotation(firePoint.position, Quaternion.identity);
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

        OnStartReload?.Invoke();
        CR_ReloadTimer = StartCoroutine(ReloadTimer(_weaponData.ReloadDuration));
    }

    void Reload()
    {
        int requestedAmmo = _magazineCapacity - _currentMagazine;

        if(requestedAmmo > _currentReserve)
        {
            _currentMagazine += _currentReserve;
            _currentReserve = 0;
        }else
        {
            _currentMagazine = _magazineCapacity;
            _currentReserve -= requestedAmmo;
        }

        OnFinishReload?.Invoke();

    }

    protected IEnumerator ReloadTimer(float duration = 1)
    {
        if(CR_Cooldown != null)
        {
            StopCoroutine(CR_Cooldown);
        }
        canFire = false;

        yield return new WaitForSeconds(duration);

        Reload();
        CR_ReloadTimer = null;

        canFire = true;

    }

    public void RestoreAmmoToReserve(int amount)
    {
        _currentReserve += amount;
        if(_currentReserve > _reserveCapacity)
        {
            _currentReserve = _reserveCapacity;
        }
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
