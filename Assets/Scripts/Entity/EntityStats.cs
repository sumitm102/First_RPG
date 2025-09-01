using UnityEngine;

public class EntityStats : MonoBehaviour
{
    public Stat maxHealth;
    public StatMajorGroup majorStats;
    public StatOffenseGroup offenseStats;
    public StatDefenseGroup defenceStats;

    public float GetMaxHealth() {
        float baseHP = maxHealth.GetValue();
        float bonusHP = majorStats.vitality.GetValue() * 5f; // Each vitality point gives +5 to max health

        return baseHP + bonusHP;
    }

    public float GetEvasion() {
        int baseEvasion = defenceStats.evasion.GetValue();
        float bonusEvasion = majorStats.agility.GetValue() * 0.5f; // Each agility point gives 50% bonus to evasion

        float totalEvasion = baseEvasion + bonusEvasion;
        float evasionLimit = 85f; // Evasion will be capped at this value (in percentage)

        return Mathf.Clamp(totalEvasion, 0f, evasionLimit);
    }
}
