using UnityEngine;

[CreateAssetMenu(menuName = "RPG Setup/Default Stat Setup", fileName = "Default Stat Setup")]
public class SO_StatSetup : ScriptableObject
{
    [Header("Resources")]
    public float maxHealth = 100f;
    public float healthRegen;

    [Header("Major")]
    public float strength;
    public float agility;
    public float intelligence;
    public float vitality;

    [Header("Offense - Physical Damage")]
    public float attackSpeed = 1f;
    public float damage = 10f;
    public float critChance;
    public float critPower = 150f;
    public float armorReduction;

    [Header("Offense - Elemental Damage")]
    public float fireDamage;
    public float iceDamage;
    public float lightningDamage;


    [Header("Defence - Physical Damage")]
    public float armor;
    public float evasion;

    [Header("Defence - Elemental Damage")]
    public float fireResistance;
    public float iceResistance;
    public float lightningResistance;


   
}
