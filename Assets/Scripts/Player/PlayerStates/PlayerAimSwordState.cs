using UnityEngine;

public class PlayerAimSwordState : PlayerState {
    public PlayerAimSwordState(Player _player, PlayerStateMachine _playerStateMachine, string _animBoolName) : base(_player, _playerStateMachine, _animBoolName) {
    }

    public override void EnterState() {
        base.EnterState();

        player.skillManager.swordSkill.DotsActive(true);

    }
    public override void UpdateState() {
        base.UpdateState();
        player.SetZeroVelocity();

        //Releasing the right click, changes state to idle
        if (Input.GetMouseButtonUp(1))
            playerStateMachine.ChangeState(player.playerIdleState);
    }

    public override void ExitState() {
        base.ExitState();
    }

}
