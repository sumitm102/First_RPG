using UnityEngine;

public class EnemyIdleState : EnemyState {
    public EnemyIdleState(Enemy _enemy, EnemyStateMachine _enemyStateMachine, string _animBoolName) : base(_enemy, _enemyStateMachine, _animBoolName) {
    }

    public override void EnterState() {
        base.EnterState();
    }
    public override void UpdateState() {
        base.UpdateState();
    }

    public override void ExitState() {
        base.ExitState();
    }

}
