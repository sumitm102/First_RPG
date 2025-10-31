using UnityEngine;

[CreateAssetMenu(menuName ="RPG Setup/Skill Data", fileName ="Skill Data - ")]
public class SO_SkillData : ScriptableObject
{
    public int cost;
    public E_SkillType skillType;
    public E_SkillUpgradeType upgradeType;

    [Header("Skill description")]
    public string skillName;

    [TextArea]
    public string description;
    public Sprite skillIcon;
}
