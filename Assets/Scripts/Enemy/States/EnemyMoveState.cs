using UnityEngine;

public class EnemyMoveState : EnemyGroundedState {
    public EnemyMoveState(StateMachine sm, int abn, Enemy e) : base(sm, abn, e) {
    }

    public override void EnterState() {
        base.EnterState();

        if (!enemy.GroundDetected || enemy.WallDetected)
            enemy.FlipCharacter();
    }
    public override void UpdateState() {
        base.UpdateState();

        enemy.SetVelocity(enemy.MoveSpeed * enemy.FacingDir, rb.linearVelocityY);

        if (!enemy.GroundDetected || enemy.WallDetected)
            stateMachine.ChangeState(enemy.IdleState);
        
    }

    public override void ExitState() {
        base.ExitState();
    }

}
