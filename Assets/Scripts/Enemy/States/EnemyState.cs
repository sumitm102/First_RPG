using UnityEngine;

public class EnemyState : EntityState {

    protected Enemy enemy;

    private static readonly int _moveAnimSpeedMultiplierHash = Animator.StringToHash("MoveAnimSpeedMultiplier");
    private static readonly int _battleAnimSpeedMultiplierHash = Animator.StringToHash("BattleAnimSpeedMultiplier");
    private static readonly int _xVelocityHash = Animator.StringToHash("xVelocity");

    public EnemyState(StateMachine sm, int abn, Enemy e) : base(sm, abn) {
        enemy = e;

        anim = enemy.Anim;
        rb = enemy.RB;
        stats = enemy.Stats;
    }

    public override void UpdateAnimationParameter() {
        base.UpdateAnimationParameter();

        float battleAnimSpeedMultiplier = enemy.BattleMoveSpeed / enemy.MoveSpeed;

        anim.SetFloat(_moveAnimSpeedMultiplierHash, enemy.MoveAnimSpeedMultiplier);
        anim.SetFloat(_battleAnimSpeedMultiplierHash, battleAnimSpeedMultiplier);
        anim.SetFloat(_xVelocityHash, rb.linearVelocityX);
    }
}
