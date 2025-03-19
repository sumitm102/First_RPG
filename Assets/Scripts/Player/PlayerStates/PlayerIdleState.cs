using UnityEngine;

public class PlayerIdleState : PlayerGroundedState
{
    public PlayerIdleState(Player _player, PlayerStateMachine _playerStateMachine, string _animBoolName) : base(_player, _playerStateMachine, _animBoolName) {
    }

    public override void EnterState() {
        base.EnterState();
    }

    public override void UpdateState() {
        base.UpdateState();
        player.SetVelocity(0, 0);

        //If player is facing the same direction as the horizontal input and if a wall detected, do nothing
        if (player.facingDir == xInput && player.IsWallDetected()) return;

        if(xInput != 0) playerStateMachine.ChangeState(player.playerMoveState);
    
    }

    public override void ExitState() {
        base.ExitState();
    }

}
