using UnityEngine;

public class PlayerCounterAttackState : PlayerState {

    private PlayerCombat _playerCombat;
    private bool _hasPerformedCounter;
    private static readonly int _counterAttackPerformedHash = Animator.StringToHash("CounterAttackPerformed");

    public PlayerCounterAttackState(StateMachine sm, int abn, Player p) : base(sm, abn, p) {
        _playerCombat = player.GetComponent<PlayerCombat>();
    }

    public override void EnterState() {
        base.EnterState();

        stateTimer = _playerCombat.CounterRecovery;

        _hasPerformedCounter = _playerCombat.CounterAttackPerformed();
        anim.SetBool(_counterAttackPerformedHash, _hasPerformedCounter);

    }
    public override void UpdateState() {
        base.UpdateState();

        player.SetVelocity(0, rb.linearVelocityY);

        if (triggerCalled)
            stateMachine.ChangeState(player.IdleState);

        if (stateTimer < 0 && !_hasPerformedCounter)
            stateMachine.ChangeState(player.IdleState);
    }

    public override void ExitState() {
        base.ExitState();
    }

    public override void UpdateAnimationParameter() {
        base.UpdateAnimationParameter();
    }

}
