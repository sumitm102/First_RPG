using UnityEngine;

public class EnemySkeleton : Enemy {

    #region States
    public SkeletonIdleState skeletonIdleState { get; private set; }
    public SkeletonMoveState skeletonMoveState { get; private set; }
    public SkeletonBattleState skeletonBattleState { get; private set; }
    public SkeletonAttackState skeletonAttackState { get; private set; }
    public SkeletonStunnedState skeletonStunnedState { get; private set; }
    public SkeletonDeadState skeletonDeadState { get; private set; }

    #endregion

    protected override void Awake() {
        base.Awake();

        skeletonIdleState = new SkeletonIdleState(this, enemyStateMachine, "Idle", this);
        skeletonMoveState = new SkeletonMoveState(this, enemyStateMachine, "Move", this);
        skeletonBattleState = new SkeletonBattleState(this, enemyStateMachine, "Move", this);
        skeletonAttackState = new SkeletonAttackState(this, enemyStateMachine, "Attack", this);
        skeletonStunnedState = new SkeletonStunnedState(this, enemyStateMachine, "Stunned", this);
        skeletonDeadState = new SkeletonDeadState(this, enemyStateMachine, "Idle", this);
    }

    protected override void Start() {
        base.Start();

        enemyStateMachine.Initialize(skeletonIdleState);
    }

    protected override void Update() {
        base.Update();

        //TODO: Remove this if statement later
        if (Input.GetKeyDown(KeyCode.H))
            enemyStateMachine.ChangeState(skeletonStunnedState);
        
    }

    public override bool CanBeStunned() {
        if (base.CanBeStunned()) {
            enemyStateMachine.ChangeState(skeletonStunnedState);
            return true;
        }

        return false;
    }

    public override void ChangeToDeadState() {
        base.ChangeToDeadState();

        enemyStateMachine.ChangeState(skeletonDeadState);
    }
}
