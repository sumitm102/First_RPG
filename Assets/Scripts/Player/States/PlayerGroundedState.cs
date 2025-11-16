using UnityEngine;

public class PlayerGroundedState : PlayerState {
    public PlayerGroundedState(StateMachine sm, int abn, Player p) : base(sm, abn, p) {
    }

    public override void EnterState() {
        base.EnterState();
    }
    public override void UpdateState() {
        base.UpdateState();


        // Using and operator to avoid flickering when character transitions from wall slide state to idle state
        if (rb.linearVelocityY < 0 && !player.GroundDetected)
            stateMachine.ChangeState(player.FallState);

        if (inputSet.Player.Jump.WasPressedThisFrame() && player.GroundDetected)
            stateMachine.ChangeState(player.JumpState);

        if (inputSet.Player.Attack.WasPressedThisFrame())
            stateMachine.ChangeState(player.BasicAttackState);

        if (inputSet.Player.CounterAttack.WasPressedThisFrame())
            stateMachine.ChangeState(player.CounterAttackState);

        if(inputSet.Player.AimSword.WasPressedThisFrame() && skillManager.SwordThrowSkill.CanUseSkill())
            stateMachine.ChangeState(player.SwordThrowState);

    }

    public override void ExitState() {
        base.ExitState();
    }

}
