using UnityEngine;

public class EntityStats : MonoBehaviour
{
    public Stat maxHealth;
    public StatMajorGroup majorStats;
    public StatOffenseGroup offenseStats;
    public StatDefenseGroup defenceStats;

    public float GetMaxHealth() {
        float baseMaxHealth = maxHealth.GetValue();
        float bonusMaxHealth = majorStats.vitality.GetValue() * 5f; // Each vitality point gives +5 to max health

        float finalMaxHealth = baseMaxHealth + bonusMaxHealth;

        return finalMaxHealth;
    }

    public float GetEvasion() {
        int baseEvasion = defenceStats.evasion.GetValue();
        float bonusEvasion = majorStats.agility.GetValue() * 0.5f; // Each agility point gives 0.5% bonus to evasion

        float totalEvasion = baseEvasion + bonusEvasion;
        float evasionLimit = 85f; // Evasion will be capped at this value (in percentage)

        float finalEvasion = Mathf.Clamp(totalEvasion, 0f, evasionLimit);

        return finalEvasion;
    }

    public float GetPhysicalDamage(out bool isCritDamage) {
        float baseDamage = offenseStats.damage.GetValue();
        float bonusDamage = majorStats.strength.GetValue(); // Each strength point gives +1 to physical damage
        float totalBaseDamage = baseDamage + bonusDamage;

        float baseCritChance = offenseStats.critChance.GetValue();
        float bonusCritChance = majorStats.agility.GetValue() * 0.3f; // Each agility point gives 0.3% bonus to crit chance
        float totalCritChance = baseCritChance + bonusCritChance;

        float baseCritPower = offenseStats.critPower.GetValue();
        float bonusCritPower = majorStats.strength.GetValue() * 0.5f; // Each strength point gives 0.5% bonus to crit power
        float totalCritPower = (baseCritPower + bonusCritPower) / 100; // Total crit power as a multiplier to the total base damage

        isCritDamage = Random.Range(0, 100f) < totalCritChance;
        float finalDamage = isCritDamage ?  totalBaseDamage * totalCritPower : totalBaseDamage;

        return finalDamage;
    }
}
