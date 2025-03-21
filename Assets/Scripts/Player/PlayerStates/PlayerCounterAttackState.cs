using UnityEngine;

public class PlayerCounterAttackState : PlayerState {
    public PlayerCounterAttackState(Player _player, PlayerStateMachine _playerStateMachine, string _animBoolName) : base(_player, _playerStateMachine, _animBoolName) {
    }

    public override void AnimationFinishTrigger() {
        base.AnimationFinishTrigger();

        player.animator.SetBool("SuccessfulCounterAttack", false);
    }

    public override void EnterState() {
        base.EnterState();

        stateTimer = player.counterAttackDuration;
    }
    public override void UpdateState() {
        base.UpdateState();

        //To avoid moving the player during attack
        player.SetZeroVelocity();

        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackCheckRadius);

        foreach(var collider in colliders) {

            if (collider.TryGetComponent<Enemy>(out Enemy enemy)) {
                if (enemy.CanBeStunned()) {
                    stateTimer = 10f; //Any value bigger than 1 to change state through animation trigger

                    player.animator.SetBool("SuccessfulCounterAttack", true);
                }
            }
        }

        //triggerCalled gets invoked from successfulCounterAttack's animation event
        if (stateTimer < 0 || triggerCalled)
            playerStateMachine.ChangeState(player.playerIdleState);


    }

    public override void ExitState() {
        base.ExitState();
    }

}
