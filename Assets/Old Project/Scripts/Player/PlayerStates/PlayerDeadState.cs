using UnityEngine;

public class PlayerDeadState : PlayerState {
    public PlayerDeadState(Player _player, PlayerStateMachine _playerStateMachine, string _animBoolName) : base(_player, _playerStateMachine, _animBoolName) {
    }


    public override void EnterState() {
        base.EnterState();
    }
    public override void UpdateState() {
        base.UpdateState();

        // Stop any kind of movement if the player is dead
        player.rbody.linearVelocity = new Vector2(0, 0);
    }

    public override void ExitState() {
        base.ExitState();
    }

    public override void AnimationFinishTrigger() {
        base.AnimationFinishTrigger();
    }
}
