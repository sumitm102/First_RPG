using UnityEngine;

public class EnemyStunnedState : EnemyState {

    private EnemyVFX _enemyVFX;

    public EnemyStunnedState(StateMachine sm, int abn, Enemy e) : base(sm, abn, e) {
        _enemyVFX = enemy.GetComponent<EnemyVFX>();

    }

    public override void EnterState() {
        base.EnterState();

        _enemyVFX.EnableAttackAlert(false);
        enemy.EnableCounterWindow(false);

        stateTimer = enemy.StunnedDuration;
        rb.linearVelocity = new Vector2(enemy.StunnedVelocity.x * -enemy.FacingDir, enemy.StunnedVelocity.y);
    }
    public override void UpdateState() {
        base.UpdateState();

        if (stateTimer < 0)
            stateMachine.ChangeState(enemy.IdleState);
    }

    public override void ExitState() {
        base.ExitState();
    }

    public override void UpdateAnimationParameter() {
        base.UpdateAnimationParameter();
    }

}
