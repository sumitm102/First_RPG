using UnityEngine;

public class EnemySkeleton : Enemy
{
    #region Anim bools

    private static readonly int _idleHash = Animator.StringToHash("Idle");
    private static readonly int _moveHash = Animator.StringToHash("Move");
    private static readonly int _attackHash = Animator.StringToHash("Attack");
    private static readonly int _battleHash = Animator.StringToHash("Battle");
    private static readonly int _deadHash = Animator.StringToHash("Dead"); // Parameter exists on animator but is currently not in use, since we don't want to apply any animation

    #endregion

    protected override void Awake() {
        base.Awake();

        IdleState = new EnemyIdleState(StateMachine, _idleHash, this);
        MoveState = new EnemyMoveState(StateMachine, _moveHash, this);
        AttackState = new EnemyAttackState(StateMachine, _attackHash, this);
        BattleState = new EnemyBattleState(StateMachine, _battleHash, this);
        DeadState = new EnemyDeadState(StateMachine, _deadHash, this);
    }
}
