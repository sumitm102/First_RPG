using UnityEngine;

public class SkillObjectSwordSpin : SkillObjectSword
{
    private int _maxDistance;
    private float _maxSpinDuration;
    private float _attacksPerSecond;
    private float _attackTimer;

    private static readonly int _spinHash = Animator.StringToHash("Spin");

    public override void SetupSword(SkillSwordThrow skilSwordThrow, Vector2 direction) {
        base.SetupSword(skilSwordThrow, direction);

        if (anim != null)
            anim.SetTrigger(_spinHash);

        _maxDistance = skillSwordThrow.maxDistance;
        _maxSpinDuration = skillSwordThrow.maxSpinDuratoin;
        _attacksPerSecond = skillSwordThrow.attacksPerSecond;

        Invoke(nameof(GetSwordBackToPlayer), _maxSpinDuration);
    }

    protected override void Update() {

        HandleAttack();
        HandleStopping();
        HandleReturn();
    }

    private void HandleStopping() {
        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

        if (distanceToPlayer > _maxDistance && rb.simulated)
            rb.simulated = false;
        }

    private void HandleAttack() {
        _attackTimer -= Time.deltaTime;

        if (_attackTimer < 0) {
            DamageEnemiesInRadius(targetCheck, checkRadius);
            _attackTimer = 1f / _attacksPerSecond;
        }
    }

    protected override void OnTriggerEnter2D(Collider2D collision) {
        rb.simulated = false;
    }
}
