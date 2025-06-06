using UnityEngine;

public class PlayerWallJumpState : PlayerState {
    public PlayerWallJumpState(Player _player, PlayerStateMachine _playerStateMachine, string _animBoolName) : base(_player, _playerStateMachine, _animBoolName) {
    }

    public override void EnterState() {
        base.EnterState();

        stateTimer = 0.4f;

        player.SetVelocity(5f * -player.facingDir, player.jumpForce);
    }
    public override void UpdateState() {
        base.UpdateState();

        if (player.IsGroundDetected())
            playerStateMachine.ChangeState(player.playerIdleState);
        if (stateTimer < 0)
            playerStateMachine.ChangeState(player.playerAirState);

    }

    public override void ExitState() {
        base.ExitState();
    }

}
