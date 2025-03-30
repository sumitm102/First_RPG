using UnityEngine;

public class PlayerGroundedState : PlayerState {

    public PlayerGroundedState(Player _player, PlayerStateMachine _playerStateMachine, string _animBoolName) : base(_player, _playerStateMachine, _animBoolName) {
    }

    public override void EnterState() {
        base.EnterState();
    }

    public override void UpdateState() {
        base.UpdateState();

        //If space key pressed and player is grounded, change to jump state
        if (Input.GetKeyDown(KeyCode.Space) && player.IsGroundDetected())
            playerStateMachine.ChangeState(player.playerJumpState);

        if (Input.GetMouseButtonDown(0))
            playerStateMachine.ChangeState(player.playerPrimaryAttackState);

        //If holding down right click and checking if the player doesn't have a sword
        if (Input.GetMouseButtonDown(1) && HasNoSword())
            playerStateMachine.ChangeState(player.playerAimSwordState);

        if (Input.GetKeyDown(KeyCode.W))
            playerStateMachine.ChangeState(player.playerCounterAttackState);

        //To make sure fall animation plays and not anything else, ie: after dashing during jump
        if (!player.IsGroundDetected())
            playerStateMachine.ChangeState(player.playerAirState);

    }

    public override void ExitState() {
        base.ExitState();
    }

    private bool HasNoSword() {
        if (!player.sword) return true;

        if (player.sword.TryGetComponent<SwordSkillController>(out SwordSkillController swordSkillController)) {
            swordSkillController.ReturnSword();
            //Debug.Log("ReturnSword Invoked");
        }

        return false;
    }


}
