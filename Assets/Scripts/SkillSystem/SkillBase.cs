using UnityEngine;

public class SkillBase : MonoBehaviour
{
    [Header("General details")]
    [SerializeField] protected E_SkillType skillType;
    [SerializeField] protected E_SkillUpgradeType upgradeType;
    [SerializeField] private float _cooldown;
    private float _lastTimeSkillUsed;


    protected virtual void Awake() {

        // To make sure skills can be used immediately after starting the game
        _lastTimeSkillUsed -= _cooldown;
    }

    public void SetSkillUpgrade(UpgradeData upgradeData) {
        upgradeType = upgradeData.upgradeType;
        _cooldown = upgradeData.cooldown;
    }

    public bool CanUseSkill() {
        if (IsSkillOnCooldown()) {
            Debug.Log("Skill is on cooldown");
            return false;
        }

        return true;
    }

    protected bool Unlocked(E_SkillUpgradeType upgradeTypeToCheck) => upgradeType == upgradeTypeToCheck;

    private bool IsSkillOnCooldown() => Time.time < _lastTimeSkillUsed + _cooldown;

    public void SetSkillOnCooldown() => _lastTimeSkillUsed = Time.time;

    public void ReduceCooldownBy(float cooldownReduction) => _lastTimeSkillUsed += cooldownReduction;

    public void ResetCooldown() => _lastTimeSkillUsed = Time.time;
}
