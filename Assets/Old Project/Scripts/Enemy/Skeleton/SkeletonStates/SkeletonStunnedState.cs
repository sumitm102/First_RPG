using UnityEngine;

public class SkeletonStunnedState : EnemyState {

    private EnemySkeleton _enemy;
    public SkeletonStunnedState(Enemy _enemyBase, EnemyStateMachine _enemyStateMachine, string _animBoolName, EnemySkeleton _enemy) : base(_enemyBase, _enemyStateMachine, _animBoolName) {
        this._enemy = _enemy;
    }

    public override void EnterState() {
        base.EnterState();

        stateTimer = _enemy.stunDuration;

        //Invoking repeatedly from enemy fx to create a red blink effect
        _enemy.fx.InvokeRepeating("RedColorBlink", 0, 0.1f);

        //Not using SetVelocity function since it also flips the enemy
        _enemy.rbody.linearVelocity = new Vector2(_enemy.stunVelocity.x * -_enemy.facingDir, _enemy.stunVelocity.y);
    }
    public override void UpdateState() {
        base.UpdateState();

        if (stateTimer < 0f)
            enemyStateMachine.ChangeState(_enemy.skeletonIdleState);
    }

    public override void ExitState() {
        base.ExitState();

        _enemy.fx.Invoke("CancelRedBlink", 0);
    }

}
