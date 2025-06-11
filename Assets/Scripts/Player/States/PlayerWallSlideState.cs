using UnityEngine;

public class PlayerWallSlideState : EntityState {
    public PlayerWallSlideState(StateMachine sm, int abn, Player p) : base(sm, abn, p) {
    }

    public override void EnterState() {
        base.EnterState();
    }
    public override void UpdateState() {
        base.UpdateState();
        HandleWallSlide();

        if (player.GroundDetected)
            player.PlayerStateMachine.ChangeState(player.IdleState);

        if (!player.WallDetected && !player.GroundDetected)
            player.PlayerStateMachine.ChangeState(player.FallState);
    }

    public override void ExitState() {
        base.ExitState();
    }

    private void HandleWallSlide() {

        // Slide downwards faster if "S" key is pressed, otherwise slide down at 30% of the original velocity
        // The horizontal input takes care of the situation where the player needs to be detached from the wall
        if (player.MoveInput.y < 0)
            player.SetVelocity(player.MoveInput.x, rb.linearVelocityY);
        else
            player.SetVelocity(player.MoveInput.x, rb.linearVelocityY * player.SlowedWallSlideMultiplier);
    }

}
