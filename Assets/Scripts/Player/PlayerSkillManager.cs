using UnityEngine;

public class PlayerSkillManager : MonoBehaviour
{
    public SkillDash DashSkill { get; private set; }
    public SkillShard ShardSkill { get; private set; }
    public SkillSwordThrow SwordThrowSkill { get; private set; }

    private void Awake() {
        DashSkill = GetComponentInChildren<SkillDash>();
        ShardSkill = GetComponentInChildren<SkillShard>();
        SwordThrowSkill = GetComponentInChildren<SkillSwordThrow>();
    }

    public SkillBase GetSkillByType(E_SkillType skillType) {
        switch (skillType) {
            case E_SkillType.Dash: return DashSkill;
            case E_SkillType.TimeShard: return ShardSkill;
            case E_SkillType.SwordThrow: return SwordThrowSkill;

            default:
                Debug.Log($"Skill type {skillType} is not implemented");
                return null;
        }
    }
}
