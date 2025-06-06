using UnityEngine;

public class PlayerCatchSwordState : PlayerState {

    private Transform _sword;

    public PlayerCatchSwordState(Player _player, PlayerStateMachine _playerStateMachine, string _animBoolName) : base(_player, _playerStateMachine, _animBoolName) {
    }

    public override void EnterState() {
        base.EnterState();

        _sword = player.sword.transform;

        //If the sword is to the left of the player and player is facing right, flip player
        if (player.transform.position.x > _sword.position.x && player.facingDir == 1f)
            player.FlipCharacter();
        //If the sword is to the right of the player and player is facing left, flip player
        else if (player.transform.position.x < _sword.position.x && player.facingDir == -1f)
            player.FlipCharacter();

        //Applying a small pushback force when catching sword
        player.rbody.linearVelocity = new Vector2(player.swordReturnImpact * -player.facingDir, player.rbody.linearVelocityY);
    }
    public override void UpdateState() {
        base.UpdateState();

        if (triggerCalled)
            playerStateMachine.ChangeState(player.playerIdleState);
    }

    public override void ExitState() {
        base.ExitState();

        player.StartCoroutine("BusyFor", 0.1f);
    }

}
