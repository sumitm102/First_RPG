using UnityEngine;

public class PlayerPrimaryAttackState : PlayerState {

    private int _comboCounter;
    private float _lastTimeAttacked;
    private float _comboWindow = 1.5f;

    public PlayerPrimaryAttackState(Player _player, PlayerStateMachine _playerStateMachine, string _animBoolName) : base(_player, _playerStateMachine, _animBoolName) {
    }

    public override void EnterState() {
        base.EnterState();

        //To make sure input stays consistent and doesn't get an unwanted value applied from another state
        xInput = 0;

        if (_comboCounter > 2 || Time.time >= _lastTimeAttacked + _comboWindow)
            _comboCounter = 0;

        //Responsible for playing animation of a combo attack 
        player.animator.SetInteger("ComboCounter", _comboCounter);

        float attackDir = player.facingDir;

        //To make sure direction of attack can be changed while in between attacks with horizontal input
        if (xInput != 0)
            attackDir = xInput;

        //Applies a force when a attack is performed to give it more weight
        player.SetVelocity(player.attackMovement[_comboCounter].x * attackDir, player.attackMovement[_comboCounter].y);

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

        //Adding a delay to stop movement from input between attacks
        player.StartCoroutine("BusyFor", 0.1f);

        _comboCounter++;
        _lastTimeAttacked = Time.time;

    }

}
