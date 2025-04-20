using UnityEngine;

public class EnemyState
{
    protected Enemy enemyBase;
    protected EnemyStateMachine enemyStateMachine;

    private string _animBoolName;

    protected bool triggerCalled;
    protected float stateTimer;

    public EnemyState(Enemy _enemyBase, EnemyStateMachine _enemyStateMachine, string _animBoolName) {
        this.enemyBase = _enemyBase;
        this.enemyStateMachine = _enemyStateMachine;
        this._animBoolName = _animBoolName;
    }

    public virtual void EnterState() {

        triggerCalled = false;
        enemyBase.animator.SetBool(_animBoolName, true);
    }

    public virtual void UpdateState() {
        stateTimer -= Time.deltaTime;
    }

    public virtual void ExitState() {
        enemyBase.animator.SetBool(_animBoolName, false);
        enemyBase.AssignLastAnimName(_animBoolName);
    }

    public void AnimationFinishTrigger() => triggerCalled = true;
}
