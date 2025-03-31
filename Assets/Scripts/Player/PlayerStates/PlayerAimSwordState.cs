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

        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        //If the cursor is to the left of the player and player is facing right, flip player
        if (player.transform.position.x > mousePosition.x && player.facingDir == 1f)
            player.FlipCharacter();
        //If the cursor is to the right of the player and player is facing left, flip player
        else if (player.transform.position.x < mousePosition.x && player.facingDir == -1f)
            player.FlipCharacter();

        //Releasing the right click, changes state to idle
        if (Input.GetMouseButtonUp(1))
            playerStateMachine.ChangeState(player.playerIdleState);
    }

    public override void ExitState() {
        base.ExitState();

        player.StartCoroutine("BusyFor", 0.2f);
    }

}
