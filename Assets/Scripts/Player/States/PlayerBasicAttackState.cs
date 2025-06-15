using UnityEngine;

public class PlayerBasicAttackState : EntityState {

    private float _attackVelocityTimer;
    public PlayerBasicAttackState(StateMachine sm, int abn, Player p) : base(sm, abn, p) {
    }

    public override void EnterState() {
        base.EnterState();
        GenerateAttackVelocity();
    }
    public override void UpdateState() {
        base.UpdateState();
        HandleAttackVelocity();

        if (triggerCalled)
            stateMachine.ChangeState(player.IdleState);
    }

    public override void ExitState() {
        base.ExitState();
    }

    private void GenerateAttackVelocity() {
        _attackVelocityTimer = player.AttackVelocityDuration;

        player.SetVelocity(player.AttackVelocity.x * player.FacingDir, player.AttackVelocity.y);
    }

    private void HandleAttackVelocity() {
        _attackVelocityTimer -= Time.deltaTime;

        if(_attackVelocityTimer < 0)
            player.SetVelocity(0, rb.linearVelocityY);

    }


}
