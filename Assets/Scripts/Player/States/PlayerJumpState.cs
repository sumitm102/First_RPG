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

        // The second condition is added to avoid transitioning to fall state while attempting to enter the jump attack state
        if (rb.linearVelocityY < 0 && stateMachine.CurrentState != player.JumpAttackState)
            stateMachine.ChangeState(player.FallState);
    }

    public override void ExitState() {
        base.ExitState();
    }

}

