using UnityEngine;

public class PlayerMoveState : PlayerState
{
    public PlayerMoveState(Player _player, PlayerStateMachine _playerStateMachine, string _animBoolName) : base(_player, _playerStateMachine, _animBoolName) {
    }

    public override void EnterState() {
        base.EnterState();
        //Debug.Log("Entered Move State");
    }

    public override void UpdateState() {
        base.UpdateState();

        if (Input.GetKeyDown(KeyCode.A)) {
            playerStateMachine.ChangeState(player.playerIdleState);
        }
    }

    public override void ExitState() {
        base.ExitState();
    }
}
