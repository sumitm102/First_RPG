using UnityEngine;

public class PlayerIdleState : PlayerGroundedState
{
    public PlayerIdleState(Player _player, PlayerStateMachine _playerStateMachine, string _animBoolName) : base(_player, _playerStateMachine, _animBoolName) {
    }

    public override void EnterState() {
        base.EnterState();
        player.SetVelocity(0, 0);
    }

    public override void UpdateState() {
        base.UpdateState();
        

        //If player is facing the same direction as the horizontal input and if a wall detected, do nothing
        if (player.facingDir == xInput && player.IsWallDetected()) return;

        //If horizontal input is detected and player is not doing something else, move the player horizontally
        if(xInput != 0 && !player.isPerformingAction) playerStateMachine.ChangeState(player.playerMoveState);
    
    }

    public override void ExitState() {
        base.ExitState();
        player.SetVelocity(0, 0);
    }

}
