using UnityEngine;

public class EnemyAnimationTriggers : EntityAnimationTriggers
{
    private Enemy _enemy;
    private EnemyVFX _enemyVFX;

    protected override void Awake() {
        base.Awake();

        _enemy = GetComponentInParent<Enemy>();
        _enemyVFX = GetComponentInParent<EnemyVFX>();
    }

    private void EnableCounterWindow() {
        _enemy.EnableCounterWindow(true);
        _enemyVFX.EnableAttackAlert(true);
    }

    private void DisableCounterWindow() {
        _enemyVFX.EnableAttackAlert(false);
        _enemy.EnableCounterWindow(false);
    }
}
