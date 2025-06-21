using UnityEngine;

public abstract class PlayerState : EntityState {
    protected Player player;
    protected PlayerInputSet inputSet;


    private static readonly int _yVelocityHash = Animator.StringToHash("yVelocity");

    public PlayerState(StateMachine sm, int abn, Player p) : base(sm, abn) {
        player = p;

        anim = player.Anim;
        rb = player.RB;
        inputSet = player.InputSet;
    }


    // EnterState gets called at first after transitioning to a new state.
    public override void EnterState() {
        base.EnterState();
    }


    // UpdateState gets called each frame and runs the logic of the current state
    public override void UpdateState() {

        base.UpdateState();

        // For updating the JumpFall blend tree based on the player's current vertical velocity
        anim.SetFloat(_yVelocityHash, rb.linearVelocityY);

        if (inputSet.Player.Dash.WasPressedThisFrame() && CanDash())
            stateMachine.ChangeState(player.DashState);
    }


    // ExitState of the current state gets called before transitioning to a new state.
    public override void ExitState() {
        base.ExitState();
    }

    private bool CanDash() {
        if (player.WallDetected || stateMachine.CurrentState == player.DashState)
            return false;

        return true;
    }
}
