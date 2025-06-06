using UnityEngine;

public class PlayerAirState : PlayerState {
    private float? _lastTimeOnGround;
    private float? _jumpButtonPressedTime;
    public PlayerAirState(Player _player, PlayerStateMachine _playerStateMachine, string _animBoolName) : base(_player, _playerStateMachine, _animBoolName) {
    }

    public override void EnterState() {
        base.EnterState();

        _lastTimeOnGround = Time.time;
    }

    public override void UpdateState() {
        base.UpdateState();

        #region Jump Buffer

        //Only make jump buffering available when player switches from move state to air state and space key is pressed
        if (playerStateMachine.previousState.Equals(player.playerMoveState) && Input.GetKeyDown(KeyCode.Space))
            _jumpButtonPressedTime = Time.time;
            

        if (Time.time - _lastTimeOnGround <= player.jumpBufferTime) {
            if (Time.time - _jumpButtonPressedTime <= player.jumpBufferTime) {
                playerStateMachine.ChangeState(player.playerJumpState);
                return;
            }
        }

        #endregion

        //To move while in air, 80% of ground movement is applied here
        if (xInput != 0)
            player.SetVelocity(player.moveSpeed * xInput * 0.8f, player.rbody.linearVelocityY);

        //If player is grounded, change to idle
        if (player.IsGroundDetected()) {
            playerStateMachine.ChangeState(player.playerIdleState);
        }

        if (player.IsWallDetected())
            playerStateMachine.ChangeState(player.playerWallSliderState);

        

    }

    public override void ExitState() {
        base.ExitState();

        _lastTimeOnGround = null;
        _jumpButtonPressedTime = null;
    }

}
