using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStats", menuName = "Scriptable Objects/PlayerStats")]
public class PlayerStats : ScriptableObject
{
    public float MaxStamina = 100f;
    public float StaminaDecreaseRate = 20f;
    public float StaminaRecoveryRate = 30f;
    public float StaminaRecoveryDelay = 2f;
    public float SlideStaminaCost = 30f;
}
