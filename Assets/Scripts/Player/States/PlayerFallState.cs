using UnityEngine;

public class PlayerFallState : PlayerAirState {
    public PlayerFallState(StateMachine sm, int abn, Player p) : base(sm, abn, p) {
    }

    public override void EnterState() {
        base.EnterState();
        //rb.gravityScale = 4f;
    }
    public override void UpdateState() {
        base.UpdateState();

        if (player.GroundDetected)
            player.PlayerStateMachine.ChangeState(player.IdleState);
    }

    public override void ExitState() {
        base.ExitState();
        //rb.gravityScale = 1f;
    }

}
