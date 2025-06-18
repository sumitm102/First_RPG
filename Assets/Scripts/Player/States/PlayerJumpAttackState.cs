using UnityEngine;

public class PlayerJumpAttackState : EntityState {

    private bool _touchedGround;
    private static readonly int _jumpAttackTriggerHash = Animator.StringToHash("JumpAttackTrigger");
    public PlayerJumpAttackState(StateMachine sm, int abn, Player p) : base(sm, abn, p) {
    }

    public override void EnterState() {
        base.EnterState();

        _touchedGround = false;
        player.SetVelocity(player.JumpAttackVelocity.x * player.FacingDir, player.JumpAttackVelocity.y);
    }
    public override void UpdateState() {
        base.UpdateState();

        if (player.GroundDetected && !_touchedGround) {
            _touchedGround = true; // To stop setting the animation trigger to true all the time
            anim.SetTrigger(_jumpAttackTriggerHash);
            player.SetVelocity(0, rb.linearVelocityY);

        }

        if (triggerCalled && player.GroundDetected)
            stateMachine.ChangeState(player.IdleState);
    }

    public override void ExitState() {
        base.ExitState();
    }

}
