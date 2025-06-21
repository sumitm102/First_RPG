using UnityEngine;

public class EnemyIdleState : EnemyState {
    public EnemyIdleState(StateMachine sm, int abn, Enemy e) : base(sm, abn, e) {
    }

    public override void EnterState() {
        base.EnterState();
        enemy.SetVelocity(0, 0);

        stateTimer = enemy.IdleTime;
    }
    public override void UpdateState() {
        base.UpdateState();

        if (stateTimer < 0)
            stateMachine.ChangeState(enemy.MoveState);

        
    }

    public override void ExitState() {
        base.ExitState();
    }

}
