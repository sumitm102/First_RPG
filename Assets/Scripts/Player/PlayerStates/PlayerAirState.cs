using UnityEngine;

public class PlayerAirState : PlayerState {
    public PlayerAirState(Player _player, PlayerStateMachine _playerStateMachine, string _animBoolName) : base(_player, _playerStateMachine, _animBoolName) {
    }

    public override void EnterState() {
        base.EnterState();
    }

    public override void UpdateState() {
        base.UpdateState();

        //If player is grounded, change to idle
        if(player.IsGroundDetected()) playerStateMachine.ChangeState(player.playerIdleState);
        

    }

    public override void ExitState() {
        base.ExitState();
    }

}
