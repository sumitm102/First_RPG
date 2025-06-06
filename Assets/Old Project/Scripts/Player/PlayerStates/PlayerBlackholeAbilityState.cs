using UnityEngine;

public class PlayerBlackholeAbilityState : PlayerState {

    private float _flightTime = 0.4f;
    private bool _skillUsed;
    private float _currentGravityScale;
    public PlayerBlackholeAbilityState(Player _player, PlayerStateMachine _playerStateMachine, string _animBoolName) : base(_player, _playerStateMachine, _animBoolName) {
    }


    public override void EnterState() {
        base.EnterState();

        _skillUsed = false;
        stateTimer = _flightTime;
        _currentGravityScale = player.rbody.gravityScale;
        player.rbody.gravityScale = 0;
    }

    public override void UpdateState() {
        base.UpdateState();

        if (stateTimer > 0) {
            player.rbody.linearVelocity = new Vector2(0, 15f);
        }
        else if (stateTimer < 0) {
            player.rbody.linearVelocity = new Vector2(0, -0.1f);

            if (!_skillUsed && player.skillManager.blackholeSkill.CanUseSkill()) 
                _skillUsed = true;
            
        }

        if (player.skillManager.blackholeSkill.IsAbilityCompleted())
            playerStateMachine.ChangeState(player.playerAirState);

        // Exiting the state and entering the air state is invoked in the BlackholeSkillController script when all of the clone attacks are performed
    }

    public override void ExitState() {
        base.ExitState();

        player.rbody.gravityScale = _currentGravityScale;
        player.MakeTransparent(false);
    }

    public override void AnimationFinishTrigger() {
        base.AnimationFinishTrigger();
    }
}
