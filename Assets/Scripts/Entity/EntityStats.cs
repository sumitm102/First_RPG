using UnityEngine;

public class EntityStats : MonoBehaviour
{
    public Stat maxHealth;
    public StatMajorGroup majorStats;
    public StatOffenseGroup offenseStats;
    public StatDefenseGroup defenceStats;

    public int GetMaxHealth() {
        int baseHP = maxHealth.GetValue();
        int bonusHP = majorStats.vitality.GetValue() * 5; // Each vitality point gives +5 to max health

        return baseHP + bonusHP;
    }
}
