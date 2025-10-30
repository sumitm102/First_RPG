using UnityEngine;

public class PlayerSkillManager : MonoBehaviour
{
    public SkillDash DashSkill { get; private set; }

    private void Awake() {
        DashSkill = GetComponentInChildren<SkillDash>();
    }
}
