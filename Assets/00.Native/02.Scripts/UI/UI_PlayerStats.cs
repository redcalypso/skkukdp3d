using UnityEngine;
using UnityEngine.UI;

public class UI_PlayerStats : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Slider _staminaSlider;

    private PlayerStats _playerStats;
    private float _currentStamina;

    public void Initialize(PlayerStats playerStats, float maxStamina)
    {
        _playerStats = playerStats;
        if (_staminaSlider != null)
        {
            _staminaSlider.maxValue = maxStamina;
            _staminaSlider.value = maxStamina;
        }
        _currentStamina = maxStamina;
    }

    public void UpdateStaminaUI(float currentStamina)
    {
        _currentStamina = currentStamina;
        
        if (_staminaSlider != null) _staminaSlider.value = _currentStamina;
    }
}
