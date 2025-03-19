using UnityEngine;

public class PlayerGroundedState : PlayerState {
    public PlayerGroundedState(Player _player, PlayerStateMachine _playerStateMachine, string _animBoolName) : base(_player, _playerStateMachine, _animBoolName) {
    }

    public override void EnterState() {
        base.EnterState();
    }

    public override void UpdateState() {
        base.UpdateState();

        if (Input.GetKeyDown(KeyCode.LeftShift))
            playerStateMachine.ChangeState(player.playerDashState);

        //If space key pressed and player is grounded, change to jump state
        if (Input.GetKeyDown(KeyCode.Space) && player.IsGroundDetected()) 
            playerStateMachine.ChangeState(player.playerJumpState);

    }

    public override void ExitState() {
        base.ExitState();
    }


}
