using UnityEngine;

public class EnemyBattleState : EnemyState {
    public EnemyBattleState(StateMachine sm, int abn, Enemy e) : base(sm, abn, e) {
    }

    public override void EnterState() {
        base.EnterState();
        Debug.Log("Battle mode");
    }
    public override void UpdateState() {
        base.UpdateState();
    }

    public override void ExitState() {
        base.ExitState();
    }

}
