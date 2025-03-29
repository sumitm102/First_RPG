using UnityEngine;

public class PlayerAnimationTriggers : MonoBehaviour
{
    private Player player => GetComponentInParent<Player>();

    private void AnimationTrigger() {
        player.AnimationTrigger();
    }

    private void AttackTrigger() {

        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackCheckRadius);

        foreach(var collider in colliders) {
            if (collider.TryGetComponent<Enemy>(out Enemy enemy))
                enemy.Damage();
            }

    }

    private void ThrowSword() {
        SkillManager.Instance.swordSkill.CreateSword();
    }
}
