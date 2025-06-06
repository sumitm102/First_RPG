using UnityEngine;

public class PlayerWallSlideState : PlayerState {
    public PlayerWallSlideState(Player _player, PlayerStateMachine _playerStateMachine, string _animBoolName) : base(_player, _playerStateMachine, _animBoolName) {
    }

    public override void EnterState() {
        base.EnterState();
    }

    public override void UpdateState() {
        base.UpdateState();

        //Change to wall jump if space is pressed while wall sliding
        if (Input.GetKeyDown(KeyCode.Space)) {
            playerStateMachine.ChangeState(player.playerWallJumpState);
            return; //Stops the rest of code to prevent unwanted situations
        }
        

        //Check for any horizontal input and see if the facing direction is not the same as that input (player wants to stop wall slide)
        if (xInput != 0 && player.facingDir != xInput)
            playerStateMachine.ChangeState(player.playerIdleState);
        
        if (player.IsGroundDetected() || !player.IsWallDetected())
            playerStateMachine.ChangeState(player.playerIdleState);

        //If player is pressing down, slide faster/at normal vertical velocity otherwise slide at 70% of that velocity
        if(yInput < 0)
            player.SetVelocity(0, player.rbody.linearVelocityY);
        else
            player.SetVelocity(0, player.rbody.linearVelocityY * 0.7f);

        

    }

    public override void ExitState() {
        base.ExitState();
    }

}
