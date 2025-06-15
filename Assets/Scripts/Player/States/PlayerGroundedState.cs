using UnityEngine;

public class PlayerGroundedState : EntityState {
    public PlayerGroundedState(StateMachine sm, int abn, Player p) : base(sm, abn, p) {
    }

    public override void EnterState() {
        base.EnterState();
    }
    public override void UpdateState() {
        base.UpdateState();


        // Using and operator to avoid flickering when player transitions from wall slide state to idle state
        if (rb.linearVelocityY < 0 && !player.GroundDetected)
            stateMachine.ChangeState(player.FallState);

        if (inputSet.Player.Jump.WasPressedThisFrame() && player.GroundDetected)
            stateMachine.ChangeState(player.JumpState);

        if (inputSet.Player.Attack.WasPressedThisFrame())
            stateMachine.ChangeState(player.BasicAttackState);

    }

    public override void ExitState() {
        base.ExitState();
    }

}
