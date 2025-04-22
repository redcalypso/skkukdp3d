using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_BombCount : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI _bombCountText;
    [SerializeField] private TextMeshProUGUI _ammoCountText;
    [SerializeField] private Image _chargeRing;
    [SerializeField] private Image _reloadRing;
    [SerializeField] private PlayerFire _playerFire;

    [Header("Ring Settings")]
    [SerializeField] private Color _chargeStartColor = Color.white;
    [SerializeField] private Color _chargeEndColor = Color.red;
    [SerializeField] private Color _reloadColor = Color.yellow;
    
    private void Start()
    {
        if (_playerFire == null) _playerFire = FindFirstObjectByType<PlayerFire>();

        InitializeRings();
        UpdateBombCountUI(_playerFire.BombCount);
    }

    private void InitializeRings()
    {
        if (_chargeRing != null) SetupRing(_chargeRing);

        if (_reloadRing != null)
        {
            SetupRing(_reloadRing);
            _reloadRing.color = _reloadColor;
        }
    }

    private void SetupRing(Image ring)
    {
        ring.type = Image.Type.Filled;
        ring.fillMethod = Image.FillMethod.Radial360;
        ring.fillOrigin = (int)Image.Origin360.Top;
        ring.fillClockwise = true;
        ring.fillAmount = 0f;
    }

    private void Update()
    {
        UpdateBombCountUI(_playerFire.BombCount);
        UpdateAmmoUI();
        UpdateChargeRing();
        UpdateReloadRing();
    }

    private void UpdateBombCountUI(int bombCount)
    {
        if (_bombCountText != null) _bombCountText.text = $"Bomb: {bombCount}/{_playerFire.MaxBombCount}";

    }

    private void UpdateAmmoUI()
    {
        if (_ammoCountText != null) _ammoCountText.text = $"{_playerFire.CurrentAmmo}/{_playerFire.MaxAmmo}";
    }

    private void UpdateChargeRing()
    {
        if (_chargeRing != null)
        {
            float progress = _playerFire.ChargeProgress;
            _chargeRing.fillAmount = progress;

            if (_playerFire.IsCharging) _chargeRing.color = Color.Lerp(_chargeStartColor, _chargeEndColor, progress);
            else _chargeRing.fillAmount = 0f;
        }
    }

    private void UpdateReloadRing()
    {
        if (_reloadRing != null)
        {
            if (_playerFire.IsReloading) _reloadRing.fillAmount = _playerFire.ReloadProgress;
            else _reloadRing.fillAmount = 0f;
        }
    }
} 