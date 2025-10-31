using UnityEngine;

public class PlayerSkillManager : MonoBehaviour
{
    public SkillDash DashSkill { get; private set; }

    private void Awake() {
        DashSkill = GetComponentInChildren<SkillDash>();
    }

    public SkillBase GetSkillByType(E_SkillType skillType) {
        switch (skillType) {
            case E_SkillType.Dash: return DashSkill;

            default:
                Debug.Log($"Skill type {skillType} is not implemented");
                return null;
        }
    }
}
