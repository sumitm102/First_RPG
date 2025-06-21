using UnityEngine;

public class PlayerAirState : PlayerState {
    public PlayerAirState(StateMachine sm, int abn, Player p) : base(sm, abn, p) {
    }

    public override void EnterState() {
        base.EnterState();
    }
    public override void UpdateState() {
        base.UpdateState();


        if (inputSet.Player.Attack.WasPressedThisFrame())
            stateMachine.ChangeState(player.JumpAttackState);
        
        //To move while in air, a certain amount of move force is applied to the player based on the multiplier
        if (player.MoveInput.x != 0)
            player.SetVelocity(player.MoveSpeed * (player.MoveInput.x * player.InAirMultiplier), rb.linearVelocityY);

    }

    public override void ExitState() {
        base.ExitState();
    }

}
