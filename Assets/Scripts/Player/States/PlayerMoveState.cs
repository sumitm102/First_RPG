using UnityEngine;

public class PlayerMoveState : PlayerGroundedState {
    public PlayerMoveState(StateMachine sm, int abn, Player p) : base(sm, abn, p) {
    }

    public override void EnterState() {
        base.EnterState();
    }


    public override void UpdateState() {
        base.UpdateState();

        if (player.MoveInput.x == 0 || player.WallDetected)
            stateMachine.ChangeState(player.IdleState);

        player.SetVelocity(player.MoveSpeed * player.MoveInput.x, rb.linearVelocityY);
    }


    public override void ExitState() {
        base.ExitState();
    }

}
