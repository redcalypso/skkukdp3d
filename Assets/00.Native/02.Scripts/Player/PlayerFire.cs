using UnityEngine;
using System.Collections;

public class PlayerFire : MonoBehaviour
{
    [Header("Prefabs and Positions")]
    public GameObject FirePosition;
    public GameObject BombPrefab;
    public ParticleSystem BulletEffectPrefab;
    public int BombCount = 3;
    public int MaxBombCount = 3;

    [Header("Pool Settings")]
    [SerializeField] private string _bombTag = "Bomb";
    [SerializeField] private string _bulletEffectTag = "BulletEffect";
    [SerializeField] private int _bombPoolSize = 5;
    [SerializeField] private int _effectPoolSize = 10;

    [Header("Gun Settings")]
    public int MaxAmmo = 50;
    public float ReloadTime = 2f;
    public float FireRate = 0.1f;
    private int _currentAmmo;
    private float _nextFireTime;
    private bool _isReloading;
    private float _reloadStartTime;

    [Header("Throw Settings")]
    public float BaseThrowPower = 15f;
    public float MaxThrowPower = 45f;
    public float MaxChargeTime = 3f;
    private float _currentThrowPower;
    private float _chargeStartTime;
    private bool _isCharging = false;

    private float _lastBombUseTime;
    private float _bombRecoveryCooldown = 3f;
    private bool _isRecovering = false;

    public float ChargeProgress => _isCharging ? Mathf.Clamp01((Time.time - _chargeStartTime) / MaxChargeTime) : 0f;
    public bool IsCharging => _isCharging;
    public float ReloadProgress => _isReloading ? Mathf.Clamp01((Time.time - _reloadStartTime) / ReloadTime) : 0f;
    public bool IsReloading => _isReloading;
    public int CurrentAmmo => _currentAmmo;

    private void Start()
    {
        _lastBombUseTime = -_bombRecoveryCooldown;
        _currentThrowPower = BaseThrowPower;
        _currentAmmo = MaxAmmo;
        InitializePools();
    }

    private void InitializePools()
    {
        Manager_ObjectPool.Instance.CreatePool(BombPrefab, _bombPoolSize);
        Manager_ObjectPool.Instance.CreatePool(BulletEffectPrefab.gameObject, _effectPoolSize);
    }

    private void Update()
    {
        HandleBombThrow();
        HandleBombRecovery();
        HandleGunFire();
        HandleReload();
    }

    private void HandleReload()
    {
        if (Input.GetKeyDown(KeyCode.R) && !_isReloading && _currentAmmo < MaxAmmo) StartReload();

        if (_isReloading)
        {
            if (Input.GetMouseButton(0))
            {
                _isReloading = false;
                return;
            }

            if (Time.time >= _reloadStartTime + ReloadTime) CompleteReload();
        }
    }

    private void StartReload()
    {
        _isReloading = true;
        _reloadStartTime = Time.time;
    }

    private void CompleteReload()
    {
        _currentAmmo = MaxAmmo;
        _isReloading = false;
    }

    private void HandleBombThrow()
    {
        if (Input.GetMouseButtonDown(1) && BombCount > 0)
        {
            _isCharging = true;
            _chargeStartTime = Time.time;
            _currentThrowPower = BaseThrowPower;
        }

        if (_isCharging && Input.GetMouseButton(1))
        {
            float chargeTime = Time.time - _chargeStartTime;
            if (chargeTime > MaxChargeTime) chargeTime = MaxChargeTime;
            float chargeRatio = chargeTime / MaxChargeTime;
            _currentThrowPower = BaseThrowPower + (MaxThrowPower - BaseThrowPower) * chargeRatio;
        }

        if (_isCharging && Input.GetMouseButtonUp(1))
        {
            _isCharging = false;
            ThrowBomb();
        }
    }

    private void ThrowBomb()
    {
        GameObject bomb = Manager_ObjectPool.Instance.Get(_bombTag, FirePosition.transform.position);
        
        if (bomb != null)
        {
            Rigidbody rb = bomb.GetComponent<Rigidbody>();
            
            if (rb != null)
            {
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                rb.AddForce(Camera.main.transform.forward * _currentThrowPower, ForceMode.Impulse);
                rb.AddTorque(Vector3.one);
            }
            
            UseBomb();
            _currentThrowPower = BaseThrowPower;
        }
    }

    private void UseBomb()
    {
        BombCount--;
        _lastBombUseTime = Time.time;
        _isRecovering = true;
    }

    private void HandleBombRecovery()
    {
        if (!_isRecovering || BombCount >= MaxBombCount) return;

        if (Time.time >= _lastBombUseTime + _bombRecoveryCooldown) RecoverBomb();
    }

    private void RecoverBomb()
    {
        BombCount = Mathf.Min(BombCount + 1, MaxBombCount);
        _lastBombUseTime = Time.time;
        
        if (BombCount >= MaxBombCount) _isRecovering = false;
    }

    private void HandleGunFire()
    {
        if (_isReloading) return;
        
        if (Input.GetMouseButton(0) && Time.time >= _nextFireTime && _currentAmmo > 0)
        {
            FireGun();
            _nextFireTime = Time.time + FireRate;
            _currentAmmo--;

            if (_currentAmmo <= 0) StartReload();
        }
    }

    private void FireGun()
    {
        if (FirePosition == null) return;
        
        Ray ray = new Ray(FirePosition.transform.position, Camera.main.transform.forward);
        
        RaycastHit hitInfo;
        
        if (Physics.Raycast(ray, out hitInfo))
        {
            GameObject effect = Manager_ObjectPool.Instance.Get(BulletEffectPrefab.name, hitInfo.point);
            
            if (effect != null)
            {
                effect.transform.rotation = Quaternion.LookRotation(hitInfo.normal);
                var particleSystem = effect.GetComponent<ParticleSystem>();
                
                if (particleSystem != null)
                {
                    effect.SetActive(true);
                    particleSystem.Clear();
                    particleSystem.Play(true);
                    
                    StartCoroutine(ReturnParticleToPool(BulletEffectPrefab.name, effect, particleSystem));
                }
            }
        }
    }

    private IEnumerator ReturnParticleToPool(string tag, GameObject obj, ParticleSystem particleSystem)
    {
        if (particleSystem == null || obj == null) yield break;

        while (particleSystem.isPlaying || particleSystem.particleCount > 0)
        {
            yield return null;
        }

        if (obj != null) Manager_ObjectPool.Instance.Return(tag, obj);
    }
}
