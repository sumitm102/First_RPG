using UnityEngine;

public class BlackholeSkill : Skill
{
    [SerializeField] private float _maxSize;
    [SerializeField] private float _growSpeed;
    [SerializeField] private float _shrinkSpeed;
    [SerializeField] private float _blackholeDuration;
    [SerializeField] private GameObject _blackholePrefab;
    [Space]
    [SerializeField] private float _cloneAttackCooldown = 0.3f;
    [SerializeField] private int _attackAmount = 4;

    private BlackholeSkillController _currentBlackhole;

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

        GameObject newBlackhole = Instantiate(_blackholePrefab, player.transform.position, Quaternion.identity);
        _currentBlackhole = newBlackhole.GetComponent<BlackholeSkillController>();

        if (_currentBlackhole != null)
            _currentBlackhole.SetupBlackhole(_maxSize, _growSpeed, _shrinkSpeed, _attackAmount, _cloneAttackCooldown, _blackholeDuration);
        
    }

    public bool IsAbilityCompleted() {
        if (_currentBlackhole != null && _currentBlackhole.playerCanExitState) {
            _currentBlackhole = null;
            return true;
        }

        return false;
    }

    public float GetBlackholeRadius() {
        return _maxSize / 2;
    }


}
