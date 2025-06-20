using UnityEngine;

public class PlayerFallState : PlayerAirState {
    public PlayerFallState(StateMachine sm, int abn, Player p) : base(sm, abn, p) {
    }

    public override void EnterState() {
        base.EnterState();
    }
    public override void UpdateState() {
        base.UpdateState();

        if (player.GroundDetected)
            stateMachine.ChangeState(player.IdleState);

        if (player.WallDetected)
            stateMachine.ChangeState(player.WallSlideState);
    }

    public override void ExitState() {
        base.ExitState();
    }

}
