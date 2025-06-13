using UnityEngine;

public class PlayerWallJumpState : EntityState {
    public PlayerWallJumpState(StateMachine sm, int abn, Player p) : base(sm, abn, p) {
    }

    public override void EnterState() {
        base.EnterState();


        player.SetVelocity(player.WallJumpForce.x * -player.FacingDir, player.WallJumpForce.y);
    }
    public override void UpdateState() {
        base.UpdateState();

        if (rb.linearVelocityY < 0)
            stateMachine.ChangeState(player.FallState);

        if (player.WallDetected)
            stateMachine.ChangeState(player.WallSlideState);
    }

    public override void ExitState() {
        base.ExitState();
    }

}
