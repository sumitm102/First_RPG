using UnityEngine;

public class PlayerAnimationTriggers : EntityAnimationTriggers
{
    private Player _player;

    protected override void Awake() {
        base.Awake();

        _player = GetComponentInParent<Player>();
    }

    private void ThrowSword() {
        _player.SkillManager.SwordThrowSkill.ThrowSword();
    }
}
