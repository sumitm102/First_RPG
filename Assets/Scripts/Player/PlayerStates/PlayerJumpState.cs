using UnityEngine;

public class PlayerJumpState : PlayerState {
    public PlayerJumpState(Player _player, PlayerStateMachine _playerStateMachine, string _animBoolName) : base(_player, _playerStateMachine, _animBoolName) {
    }

    public override void EnterState() {
        base.EnterState();
        
        //Applies upward force on entering the state, not changing the horizontal velocity
        player.playerRigidbody.linearVelocity = new Vector2(player.playerRigidbody.linearVelocityX, player.jumpForce);
    }

    public override void UpdateState() {
        base.UpdateState();

        //If player is falling, change to air state
        if (player.playerRigidbody.linearVelocityY < 0f) playerStateMachine.ChangeState(player.playerAirState);
   

    }

    public override void ExitState() {
        base.ExitState();
    }
}
