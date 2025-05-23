using UnityEngine;

public class EnemySkeletonAnimationTriggers : MonoBehaviour
{
    private EnemySkeleton _enemy => GetComponentInParent<EnemySkeleton>();

    private void AnimationTrigger() {
        _enemy.AnimationFinishTrigger();
    }

    private void AttackTrigger() {

        Collider2D[] colliders = Physics2D.OverlapCircleAll(_enemy.attackCheck.position, _enemy.attackCheckRadius);

        foreach(var collider in colliders) {
            if (collider.TryGetComponent<Player>(out Player player))
                _enemy.characterStats.InflictDamage(player.characterStats);
        }
    }

    private void OpenCounterAttackWindow() => _enemy.OpenCounterAttackWindow();
    private void CloseCounterAttackWindow() => _enemy.CloseCounterAttackWindow();
}
