using UnityEngine;

public class PlayerDashState : PlayerState
{
    public PlayerDashState(Player _player, PlayerStateMachine _playerStateMachine, string _animBoolName) : base(_player, _playerStateMachine, _animBoolName) {
    }

    public override void EnterState() {
        base.EnterState();

        stateTimer = player.dashDuration; //stateTimer inherited from PlayerState
    }

    public override void UpdateState() {
        base.UpdateState();

        //Apply dash speed on top of player's current speed
        player.SetVelocity(player.facingDir * player.dashSpeed, player.playerRigidbody.linearVelocityY);


        //Even if it switches to idle, it'll switch to another state from idle upon player input or certain conditions
        if (stateTimer < 0f) playerStateMachine.ChangeState(player.playerIdleState);
    }

    public override void ExitState() {
        base.ExitState();

        //Setting the horizontal movement to 0 upon exiting to avoid moving indefinitely
        player.SetVelocity(0f, player.playerRigidbody.linearVelocityY);
    }

}

