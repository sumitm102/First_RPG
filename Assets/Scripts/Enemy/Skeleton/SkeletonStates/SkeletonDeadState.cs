using UnityEngine;

public class SkeletonDeadState : EnemyState {

    private EnemySkeleton _enemy;

    public SkeletonDeadState(Enemy _enemyBase, EnemyStateMachine _enemyStateMachine, string _animBoolName, EnemySkeleton _enemy) : base(_enemyBase, _enemyStateMachine, _animBoolName) {
        this._enemy = _enemy;
    }

    public override void EnterState() {
        base.EnterState();

        // Play the last animation beofre dying, stop it, and turn off collider to fall off the map
        _enemy.animator.SetBool(_enemy.lastAnimBoolName, true);
        _enemy.animator.speed = 0;
        _enemy.capsuleCollider.enabled = false;

        stateTimer = 0.15f;
    }
    public override void UpdateState() {
        base.UpdateState();

        //Apply some force for a brief moment
        if (stateTimer > 0)
            _enemy.rbody.linearVelocity = new Vector2(PlayerManager.Instance.player.facingDir * 5f, 10f);
    }

    public override void ExitState() {
        base.ExitState();
    }

}
