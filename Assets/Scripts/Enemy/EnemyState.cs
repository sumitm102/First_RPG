using UnityEngine;

public class EnemyState : EntityState {

    protected Enemy enemy;

    private static readonly int _moveAnimSpeedMultiplier = Animator.StringToHash("MoveAnimSpeedMultiplier");
    public EnemyState(StateMachine sm, int abn, Enemy e) : base(sm, abn) {
        enemy = e;

        anim = enemy.Anim;
        rb = enemy.RB;
    }

    public override void UpdateState() {
        base.UpdateState();

        anim.SetFloat(_moveAnimSpeedMultiplier, enemy.MoveAnimSpeedMultiplier);
    }
}
