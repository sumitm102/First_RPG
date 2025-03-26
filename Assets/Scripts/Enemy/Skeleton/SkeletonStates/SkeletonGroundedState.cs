using UnityEngine;

public class SkeletonGroundedState : EnemyState {
    protected EnemySkeleton enemy;
    protected Transform player;

    public SkeletonGroundedState(Enemy _enemyBase, EnemyStateMachine _enemyStateMachine, string _animBoolName, EnemySkeleton _enemy) : base(_enemyBase, _enemyStateMachine, _animBoolName) {
        this.enemy = _enemy;
    }

    public override void EnterState() {
        base.EnterState();
        player = PlayerManager.Instance.player.transform;

    }
    public override void UpdateState() {
        base.UpdateState();

        //If played is detected or the distance between is less than the close range value (needed for detecting player while facing back) then switch to battle state
        if (enemy.IsPlayerDetected() || Vector2.Distance(player.transform.position, enemy.transform.position) < 2f) 
            enemyStateMachine.ChangeState(enemy.skeletonBattleState);
        
    }

    public override void ExitState() {
        base.ExitState();
    }

}
