using UnityEngine;

public class PlayerAnimationTriggers : MonoBehaviour
{
    private Player _player => GetComponentInParent<Player>();

    private void AnimationTrigger() {
        _player.AnimationTrigger();
    }

    private void AttackTrigger() {

        Collider2D[] colliders = Physics2D.OverlapCircleAll(_player.attackCheck.position, _player.attackCheckRadius);

        foreach(var collider in colliders) {
            if (collider.TryGetComponent<Enemy>(out Enemy enemy)) {
                _player.characterStats.InflictDamage(enemy.characterStats);
            }
                
        }      

    }

    private void ThrowSword() {
        SkillManager.Instance.swordSkill.CreateSword();
    }
}
