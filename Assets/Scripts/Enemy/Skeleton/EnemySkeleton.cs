using UnityEngine;

public class EnemySkeleton : Enemy
{
    #region Anim bools

    private static readonly int _idleHash = Animator.StringToHash("Idle");
    private static readonly int _moveHash = Animator.StringToHash("Move");

    #endregion

    protected override void Awake() {
        base.Awake();

        IdleState = new EnemyIdleState(StateMachine, _idleHash, this);
        MoveState = new EnemyMoveState(StateMachine, _moveHash, this);
    }
}
