using UnityEngine;

public class PlayerIdleState : EntityState {
    public PlayerIdleState(StateMachine sm, string sn, Player p) : base(sm, sn, p) {
    }

    public override void EnterState() {
        base.EnterState();
    }


    public override void UpdateState() {
        base.UpdateState();

        if (player.MoveInput.x != 0)
            player.PlayerStateMachine.ChangeState(player.MoveState);
    }


    public override void ExitState() {
        base.ExitState();
    }
}
