using UnityEngine;

public class PlayerJumpState : PlayerAirState {
    public PlayerJumpState(StateMachine sm, int abn, Player p) : base(sm, abn, p) {
    }

    public override void EnterState() {
        base.EnterState();

        player.SetVelocity(rb.linearVelocityX, player.JumpForce);
    }
    public override void UpdateState() {
        base.UpdateState();

        if (rb.linearVelocityY < 0)
            player.PlayerStateMachine.ChangeState(player.FallState);
    }

    public override void ExitState() {
        base.ExitState();
    }

}

