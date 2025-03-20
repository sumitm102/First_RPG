using UnityEngine;

public class EnemyState
{
    public Enemy enemy { get; private set; }
    public EnemyStateMachine enemyStateMachine { get; private set; }
    public string _animBoolName { get; private set; }

    public EnemyState(Enemy _enemy, EnemyStateMachine _enemyStateMachine, string _animBoolName) {
        this.enemy = _enemy;
        this.enemyStateMachine = _enemyStateMachine;
        this._animBoolName = _animBoolName;
    }

    public virtual void EnterState() {
        enemy.enemyAnimator.SetBool(_animBoolName, true);
    }

    public virtual void UpdateState() {

    }

    public virtual void ExitState() {
        enemy.enemyAnimator.SetBool(_animBoolName, false);
    }
}
