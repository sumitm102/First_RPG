using UnityEngine;

public class SkeletonAttackState : EnemyState {
    private EnemySkeleton _enemy;

    public SkeletonAttackState(Enemy _enemyBase, EnemyStateMachine _enemyStateMachine, string _animBoolName, EnemySkeleton _enemy) : base(_enemyBase, _enemyStateMachine, _animBoolName) {
        this._enemy = _enemy;
    }

    public override void EnterState() {
        base.EnterState();
    }

    public override void UpdateState() {
        base.UpdateState();

        //Stopping enemy to move during attack unless knocked back
        _enemy.SetZeroVelocity();

        if (triggerCalled)
            enemyStateMachine.ChangeState(_enemy.skeletonBattleState);

        
    }

    public override void ExitState() {
        base.ExitState();

        _enemy.lastTimeAttacked = Time.time;
    }

}
