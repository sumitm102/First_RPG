using UnityEngine;

public class EnemyAttackState : EnemyState {
    public EnemyAttackState(StateMachine sm, int abn, Enemy e) : base(sm, abn, e) {
    }

    public override void EnterState() {
        base.EnterState();
    }
    public override void UpdateState() {
        base.UpdateState();
    }

    public override void ExitState() {
        base.ExitState();

        if (triggerCalled)
            stateMachine.ChangeState(enemy.IdleState);
    }

}
