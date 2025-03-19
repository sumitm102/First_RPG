using UnityEngine;

public class PlayerPrimaryAttackState : PlayerState {

    private int _comboCounter;
    private float _lastTimeAttacked;
    private float _comboWindow = 2f;

    public PlayerPrimaryAttackState(Player _player, PlayerStateMachine _playerStateMachine, string _animBoolName) : base(_player, _playerStateMachine, _animBoolName) {
    }

    public override void EnterState() {
        base.EnterState();

        if (_comboCounter > 2 || Time.time >= _lastTimeAttacked + _comboWindow)
            _comboCounter = 0;

        player.playerAnimator.SetInteger("ComboCounter", _comboCounter);

        stateTimer = 0.1f;
    }

    public override void UpdateState() {
        base.UpdateState();

        if(stateTimer < 0)
            player.SetVelocity(0, 0);

        if (triggerCalled)
            playerStateMachine.ChangeState(player.playerIdleState);


    }

    public override void ExitState() {
        base.ExitState();

        player.StartCoroutine("BusyFor", 0.1f);

        _comboCounter++;
        _lastTimeAttacked = Time.time;

    }

}
