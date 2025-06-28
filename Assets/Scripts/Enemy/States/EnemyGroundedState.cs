using UnityEngine;

public class EnemyGroundedState : EnemyState {
    public EnemyGroundedState(StateMachine sm, int abn, Enemy e) : base(sm, abn, e) {
    }

    public override void EnterState() {
        base.EnterState();
    }
    public override void UpdateState() {
        base.UpdateState();

        if (enemy.PlayerDetection())
            stateMachine.ChangeState(enemy.BattleState);
        
    }

    public override void ExitState() {
        base.ExitState();
    }

}
