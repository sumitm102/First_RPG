using UnityEngine;

public class PlayerAirState : PlayerState {
    public PlayerAirState(Player _player, PlayerStateMachine _playerStateMachine, string _animBoolName) : base(_player, _playerStateMachine, _animBoolName) {
    }

    public override void EnterState() {
        base.EnterState();
    }

    public override void UpdateState() {
        base.UpdateState();

        //To move while in air, 80% of ground movement is applied here
        if (xInput != 0)
            player.SetVelocity(player.moveSpeed * xInput * 0.8f, player.playerRigidbody.linearVelocityY);

        //If player is grounded, change to idle
        if (player.IsGroundDetected()) {
            playerStateMachine.ChangeState(player.playerIdleState);
            return;
        }

        if (player.IsWallDetected())
            playerStateMachine.ChangeState(player.playerWallSliderState);

        

    }

    public override void ExitState() {
        base.ExitState();
    }

}
