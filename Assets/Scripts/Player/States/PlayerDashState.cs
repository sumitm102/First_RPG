using UnityEngine;

public class PlayerDashState : PlayerState {

    private float _originalGravityScale;
    private int _dashDir;

    public PlayerDashState(StateMachine sm, int abn, Player p) : base(sm, abn, p) {
    }

    public override void EnterState() {
        base.EnterState();

        _dashDir = player.MoveInput.x != 0 ? ((int)player.MoveInput.x) : player.FacingDir;
        stateTimer = player.DashDuration;

        _originalGravityScale = rb.gravityScale;
        rb.gravityScale = 0;

    }

    public override void UpdateState() {
        base.UpdateState();
        CancelDashWhenWallDetected();

        player.SetVelocity(player.DashSpeed * _dashDir, 0);

        if (stateTimer < 0) {
            if (player.GroundDetected)
                stateMachine.ChangeState(player.IdleState);
            else
                stateMachine.ChangeState(player.FallState);
        }
    }

    public override void ExitState() {
        base.ExitState();

        rb.gravityScale = _originalGravityScale;
        player.SetVelocity(0, 0);
    }

    private void CancelDashWhenWallDetected() {
        if (player.WallDetected) {
            if (player.GroundDetected) 
                stateMachine.ChangeState(player.IdleState);
            else
                stateMachine.ChangeState(player.WallSlideState);
        }

    }
    
}
