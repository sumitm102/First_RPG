using UnityEngine;

public class PlayerMoveState : PlayerGroundedState
{
    public PlayerMoveState(Player _player, PlayerStateMachine _playerStateMachine, string _animBoolName) : base(_player, _playerStateMachine, _animBoolName) {
    }

    public override void EnterState() {
        base.EnterState();
        //Debug.Log("Entered Move State");
    }

    public override void UpdateState() {
        base.UpdateState();

        player.SetVelocity(xInput * player.moveSpeed, player.playerRigidbody.linearVelocityY);

        if (xInput == 0f) playerStateMachine.ChangeState(player.playerIdleState);
        
    }

    public override void ExitState() {
        base.ExitState();
    }
}
