using UnityEngine;

public class BlackholeSkill : Skill
{
    [SerializeField] private float _maxSize;
    [SerializeField] private float _growSpeed;
    [SerializeField] private float _shrinkSpeed;
    [SerializeField] private GameObject _blackholePrefab;
    [Space]
    [SerializeField] private float _cloneAttackCooldown = 0.3f;
    [SerializeField] private int _attackAmount = 4;

    protected override void Start() {
        base.Start();
    }
    protected override void Update() {
        base.Update();
    }

    public override bool CanUseSkill() {
        return base.CanUseSkill();
    }

    public override void UseSkill() {
        base.UseSkill();

        GameObject newBlackhole = Instantiate(_blackholePrefab);

        if(newBlackhole.TryGetComponent<BlackholeSkillController>(out BlackholeSkillController blackholeSkillController)) {
            blackholeSkillController.SetupBlackhole(_maxSize, _growSpeed, _shrinkSpeed, _attackAmount, _cloneAttackCooldown);
        }
    }


}
