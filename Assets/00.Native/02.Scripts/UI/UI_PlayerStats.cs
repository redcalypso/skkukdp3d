using UnityEngine;
using UnityEngine.UI;

public class UI_PlayerStats : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Slider staminaSlider;
    [SerializeField] private Image staminaFillImage;

    private PlayerStats _playerStats;
    private float _currentStamina;

    public void Initialize(PlayerStats playerStats, float maxStamina)
    {
        _playerStats = playerStats;
        if (staminaSlider != null)
        {
            staminaSlider.maxValue = maxStamina;
            staminaSlider.value = maxStamina;
        }
        _currentStamina = maxStamina;
    }

    public void UpdateStaminaUI(float currentStamina)
    {
        _currentStamina = currentStamina;
        
        if (staminaSlider != null)
        {
            staminaSlider.value = _currentStamina;
        }

        if (staminaFillImage != null)
        {
            float ratio = _currentStamina / _playerStats.MaxStamina;
            Color targetColor;

            if (ratio > 0.5f)
                targetColor = Color.green;
            else if (ratio > 0.2f)
                targetColor = Color.yellow;
            else
                targetColor = Color.red;

            staminaFillImage.color = Color.Lerp(staminaFillImage.color, targetColor, Time.deltaTime * 10f);
        }
    }
}
