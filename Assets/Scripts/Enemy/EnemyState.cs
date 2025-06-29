using UnityEngine;

public class EnemyState : EntityState {

    protected Enemy enemy;

    private static readonly int _moveAnimSpeedMultiplierHash = Animator.StringToHash("MoveAnimSpeedMultiplier");
    private static readonly int _xVelocityHash = Animator.StringToHash("xVelocity");

    public EnemyState(StateMachine sm, int abn, Enemy e) : base(sm, abn) {
        enemy = e;

        anim = enemy.Anim;
        rb = enemy.RB;
    }

    public override void UpdateState() {
        base.UpdateState();

        anim.SetFloat(_moveAnimSpeedMultiplierHash, enemy.MoveAnimSpeedMultiplier);
        anim.SetFloat(_xVelocityHash, rb.linearVelocityX);
    }
}
