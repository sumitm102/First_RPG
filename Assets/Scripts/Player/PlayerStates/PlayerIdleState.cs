using UnityEngine;

public class PlayerIdleState : PlayerState
{
    public PlayerIdleState(Player _player, PlayerStateMachine _playerStateMachine, string _animBoolName) : base(_player, _playerStateMachine, _animBoolName) {
    }

    public override void EnterState() {
        base.EnterState();
    }

    public override void UpdateState() {
        base.UpdateState();

        if (Input.GetKeyDown(KeyCode.D)) {
            playerStateMachine.ChangeState(player.playerMoveState);
        }
    }

    public override void ExitState() {
        base.ExitState();
    }

}
