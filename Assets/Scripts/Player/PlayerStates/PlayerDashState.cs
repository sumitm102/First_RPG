using UnityEngine;

public class PlayerDashState : PlayerState
{
    public PlayerDashState(Player _player, PlayerStateMachine _playerStateMachine, string _animBoolName) : base(_player, _playerStateMachine, _animBoolName) {
    }

    public override void EnterState() {
        base.EnterState();

        //Creates a clone as the character start dashing
        player.skillManager.cloneSkill.CreateClone(player.transform);

        stateTimer = player.dashDuration; //stateTimer inherited from PlayerState
    }

    public override void UpdateState() {
        base.UpdateState();

        //Even if it switches to idle, it'll switch to another state from idle upon player input or certain conditions
        if (stateTimer < 0f) {
            playerStateMachine.ChangeState(player.playerIdleState);
            return; //To stop applying any velocity in case of undesired movement
        }

        //Switching to wall slide immediately after dashing in air and detecting a wall
        if (!player.IsGroundDetected() && player.IsWallDetected())
            playerStateMachine.ChangeState(player.playerWallSliderState);

        //Apply dash speed and set y to 0 to not lose vertical velocity or stay on air
        player.SetVelocity(player.dashDir * player.dashSpeed, 0);
    }

    public override void ExitState() {
        base.ExitState();

        //Setting the horizontal movement to 0 upon exiting to avoid moving indefinitely
        player.SetVelocity(0f, player.rbody.linearVelocityY);
    }

}

