using UnityEngine;

public class PlayerIdleState : PlayerGroundedState {
    public PlayerIdleState(StateMachine sm, int abn, Player p) : base(sm, abn, p) {
    }

    public override void EnterState() {
        base.EnterState();

        // Reset horizontal velocity to zero to avoid sliding
        player.SetVelocity(0, rb.linearVelocityY);
    }


    public override void UpdateState() {
        base.UpdateState();

        // To stop transitioning to move state when the moving character detects a wall
        if (player.MoveInput.x == player.FacingDir && player.WallDetected)
            return;
       
        if (player.MoveInput.x != 0)
            stateMachine.ChangeState(player.MoveState);

    }


    public override void ExitState() {
        base.ExitState();

        // To avoid facing left when character transitions to jump state from this one even if they are facing right
        player.SetVelocity(0, rb.linearVelocityY);
    }
}
