using UnityEngine;

public class SkeletonBattleState : EnemyState {
    private EnemySkeleton _enemy;
    private Transform _player;
    private int _moveDir;
    public SkeletonBattleState(Enemy _enemyBase, EnemyStateMachine _enemyStateMachine, string _animBoolName, EnemySkeleton _enemy) : base(_enemyBase, _enemyStateMachine, _animBoolName) {
        this._enemy = _enemy;
    }

    public override void EnterState() {
        base.EnterState();

        _player = GameObject.FindWithTag("Player").transform;
    }

    public override void UpdateState() {
        base.UpdateState();

        //Checking if player is to the right of the enemy using world positions
        if (_player.position.x > _enemy.transform.position.x)
            _moveDir = 1;
        //Checking if player is to the left of the enemy using world positions
        else if (_player.position.x < _enemy.transform.position.x)
            _moveDir = -1;

        _enemy.SetVelocity(_enemy.moveSpeed * _moveDir, _enemy.rbody.linearVelocityY);

        //If player is present, within attack range and attack is not on cooldonw then change to attack state
        if (_enemy.IsPlayerDetected()) {
            stateTimer = _enemy.battleTime;

            if (_enemy.IsPlayerDetected().distance < _enemy.attackDistance) {
                if(CanAttack())
                    enemyStateMachine.ChangeState(_enemy.skeletonAttackState);
            }
        }
        else {

            //Change to idle if player is not detected and battle duration goes to negative or distance between the player and itself is greater than its detection range
            if (stateTimer < 0 || Vector2.Distance(_player.transform.position, _enemy.transform.position) > _enemy.detectionRange)
                enemyStateMachine.ChangeState(_enemy.skeletonIdleState);
        }
            

      
               
    }

    public override void ExitState() {
        base.ExitState();
    }

    private bool CanAttack() {
        if (Time.time > _enemy.lastTimeAttacked + _enemy.attackCooldown) {
            _enemy.lastTimeAttacked = Time.time;
            return true;
        }

        return false;
    }

}
