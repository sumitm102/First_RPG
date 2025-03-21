using UnityEngine;

public class SkeletonIdleState : SkeletonGroundedState {
    public SkeletonIdleState(Enemy _enemyBase, EnemyStateMachine _enemyStateMachine, string _animBoolName, EnemySkeleton _enemy) : base(_enemyBase, _enemyStateMachine, _animBoolName, _enemy) {
    }

    public override void EnterState() {
        base.EnterState();

        enemy.SetVelocity(0, 0);
        stateTimer = enemy.idleTime;
    }
    public override void UpdateState() {
        base.UpdateState();

        if (stateTimer < 0) 
            enemyStateMachine.ChangeState(enemy.skeletonMoveState);
    }

    public override void ExitState() {
        base.ExitState();
    }

}
