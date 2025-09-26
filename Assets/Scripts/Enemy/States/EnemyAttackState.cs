using UnityEngine;

public class EnemyAttackState : EnemyState {
    public EnemyAttackState(StateMachine sm, int abn, Enemy e) : base(sm, abn, e) {
    }

    public override void EnterState() {
        base.EnterState();
        SyncAttackSpeed();
    }
    public override void UpdateState() {
        base.UpdateState();

        if (triggerCalled)
            stateMachine.ChangeState(enemy.BattleState);
    }

    public override void ExitState() {
        base.ExitState();
    }

}
