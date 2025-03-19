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

        if(xInput != 0) playerStateMachine.ChangeState(player.playerMoveState);
        //player.playerRigidbody.linearVelocityX = 0f;
    
    }

    public override void ExitState() {
        base.ExitState();
    }

}
