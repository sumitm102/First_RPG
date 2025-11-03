using UnityEngine;
using System;

[CreateAssetMenu(menuName ="RPG Setup/Skill Data", fileName ="Skill Data - ")]
public class SO_SkillData : ScriptableObject
{
    public int cost;
    public bool isUnlockedByDefault;
    public E_SkillType skillType;
    public UpgradeData upgradeData;

    [Header("Skill description")]
    public string skillName;

    [TextArea]
    public string description;
    public Sprite skillIcon;
}

[Serializable]
public class UpgradeData {
    public E_SkillUpgradeType upgradeType;
    public float cooldown;
}
