using UnityEngine;

public class SkillBase : MonoBehaviour
{
    public Player Player {  get; private set; }
    public DamageScaleData DamageScaleData { get; private set; }

    [Header("General details")]
    [SerializeField] protected E_SkillType skillType;
    [SerializeField] protected E_SkillUpgradeType upgradeType;
    [SerializeField] protected float cooldown;
    private float _lastTimeSkillUsed;


    protected virtual void Awake() {

        // To make sure skills can be used immediately after starting the game
        _lastTimeSkillUsed -= cooldown;
        Player = GetComponentInParent<Player>();
    }

    public virtual void TryUseSkill() {

    }

    public void SetSkillUpgrade(UpgradeData upgradeData) {
        upgradeType = upgradeData.upgradeType;
        cooldown = upgradeData.cooldown;
        DamageScaleData = upgradeData.damageScaleData;
    }

    public bool CanUseSkill() {
        if (upgradeType == E_SkillUpgradeType.None) 
            return false;

        if (IsSkillOnCooldown()) {
            Debug.Log("Skill is on cooldown");
            return false;
        }

        return true;
    }

    protected bool IsUpgradeUnlocked(E_SkillUpgradeType upgradeTypeToCheck) => upgradeType == upgradeTypeToCheck;

    protected bool IsSkillOnCooldown() => Time.time < _lastTimeSkillUsed + cooldown;

    public void SetSkillOnCooldown() => _lastTimeSkillUsed = Time.time;

    public void ReduceCooldownBy(float cooldownReduction) => _lastTimeSkillUsed += cooldownReduction;

    public void ResetCooldown() => _lastTimeSkillUsed = Time.time;
}
