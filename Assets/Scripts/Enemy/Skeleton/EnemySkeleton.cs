using UnityEngine;

public class EnemySkeleton : Enemy {

    #region States
    public SkeletonIdleState skeletonIdleState { get; private set; }
    public SkeletonMoveState skeletonMoveState { get; private set; }
    public SkeletonBattleState skeletonBattleState { get; private set; }
    public SkeletonAttackState skeletonAttackState { get; private set; }

    #endregion

    protected override void Awake() {
        base.Awake();

        skeletonIdleState = new SkeletonIdleState(this, enemyStateMachine, "Idle", this);
        skeletonMoveState = new SkeletonMoveState(this, enemyStateMachine, "Move", this);
        skeletonBattleState = new SkeletonBattleState(this, enemyStateMachine, "Move", this);
        skeletonAttackState = new SkeletonAttackState(this, enemyStateMachine, "Attack", this);
    }

    protected override void Start() {
        base.Start();

        enemyStateMachine.Initialize(skeletonIdleState);
    }

    protected override void Update() {
        base.Update();
    }
}
