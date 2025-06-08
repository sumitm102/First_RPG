using UnityEngine;

public class PlayerIdleState : EntityState {
    public PlayerIdleState(StateMachine sm, int abn, Player p) : base(sm, abn, p) {
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
