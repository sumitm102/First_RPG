using UnityEngine;

public class SkeletonIdleState : EnemyState {

    private EnemySkeleton _enemy;

    public SkeletonIdleState(Enemy _enemyBase, EnemyStateMachine _enemyStateMachine, string _animBoolName, EnemySkeleton _enemy) : base(_enemy, _enemyStateMachine, _animBoolName) {
        this._enemy = _enemy;

    }

    public override void EnterState() {
        base.EnterState();

        _enemy.SetVelocity(0, 0);
        stateTimer = _enemy.idleTime;
    }
    public override void UpdateState() {
        base.UpdateState();

        if (stateTimer < 0) 
            enemyStateMachine.ChangeState(_enemy.skeletonMoveState);
    }

    public override void ExitState() {
        base.ExitState();
    }

}
