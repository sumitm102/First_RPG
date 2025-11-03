using UnityEngine;

public class PlayerSkillManager : MonoBehaviour
{
    public SkillDash DashSkill { get; private set; }
    public SkillShard ShardSkill { get; private set; }

    private void Awake() {
        DashSkill = GetComponentInChildren<SkillDash>();
        ShardSkill = GetComponentInChildren<SkillShard>();
    }

    public SkillBase GetSkillByType(E_SkillType skillType) {
        switch (skillType) {
            case E_SkillType.Dash: return DashSkill;
            case E_SkillType.TimeShard: return ShardSkill;

            default:
                Debug.Log($"Skill type {skillType} is not implemented");
                return null;
        }
    }
}
