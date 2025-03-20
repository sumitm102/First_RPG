using UnityEngine;

public class SkeletonMoveState : EnemyState {

    private EnemySkeleton _enemy;

    public SkeletonMoveState(Enemy _enemyBase, EnemyStateMachine _enemyStateMachine, string _animBoolName, EnemySkeleton _enemy) : base(_enemy, _enemyStateMachine, _animBoolName) {
        this._enemy = _enemy;
    }

    public override void EnterState() {
        base.EnterState();
    }
    public override void UpdateState() {
        base.UpdateState();

        _enemy.SetVelocity(_enemy.moveSpeed * _enemy.facingDir, _enemy.rbody.linearVelocityY);

        if (_enemy.IsWallDetected() || !_enemy.IsGroundDetected()) {
            _enemy.FlipCharacter();
            enemyStateMachine.ChangeState(_enemy.skeletonIdleState);
        }
    }

    public override void ExitState() {
        base.ExitState();
    }
}
