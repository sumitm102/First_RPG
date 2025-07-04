using UnityEngine;

public class PlayerDeadState : PlayerState {
    public PlayerDeadState(StateMachine sm, int abn, Player p) : base(sm, abn, p) {
    }

    public override void EnterState() {
        base.EnterState();

        inputSet.Disable();
        stateMachine.PreventChangingToNewState();
        rb.simulated = false;
    }
    public override void UpdateState() {
        base.UpdateState();
    }

    public override void ExitState() {
        base.ExitState();
    }

}
