using UnityEngine;

public class SkeletonMoveState : SkeletonGroundedState {
    public SkeletonMoveState(Enemy _enemyBase, EnemyStateMachine _enemyStateMachine, string _animBoolName, EnemySkeleton _enemy) : base(_enemyBase, _enemyStateMachine, _animBoolName, _enemy) {
    }

    public override void EnterState() {
        base.EnterState();
    }

    public override void UpdateState() {
        base.UpdateState();

        enemy.SetVelocity(enemy.moveSpeed * enemy.facingDir, enemy.rbody.linearVelocityY);

        if (enemy.IsWallDetected() || !enemy.IsGroundDetected()) {
            enemy.FlipCharacter();
            enemyStateMachine.ChangeState(enemy.skeletonIdleState);
        }
    }

    public override void ExitState() {
        base.ExitState();
    }
}
