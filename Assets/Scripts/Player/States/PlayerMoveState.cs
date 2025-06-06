using UnityEngine;

public class PlayerMoveState : EntityState {
    public PlayerMoveState(StateMachine sm, string sn, Player p) : base(sm, sn, p) {
    }

    public override void EnterState() {
        base.EnterState();
    }


    public override void UpdateState() {
        base.UpdateState();

        if (player.MoveInput.x == 0)
            player.PlayerStateMachine.ChangeState(player.IdleState);
    }


    public override void ExitState() {
        base.ExitState();
    }

}
