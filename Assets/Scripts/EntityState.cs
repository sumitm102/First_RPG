using UnityEngine;

public abstract class EntityState {
    protected Player player;
    protected StateMachine stateMachine;
    protected int animBoolName;

    protected Animator anim;
    protected Rigidbody2D rb;
    protected PlayerInputSet inputSet;

    protected float stateTimer;
    protected bool triggerCalled;

    private static readonly int _yVelocityHash = Animator.StringToHash("yVelocity");

    public EntityState(StateMachine sm, int abn, Player p) {
        stateMachine = sm;
        animBoolName = abn;
        player = p;

        anim = player.Anim;
        rb = player.RB;
        inputSet = player.InputSet;
    }


    // EnterState gets called at first after transitioning to a new state.
    public virtual void EnterState() {
        anim.SetBool(animBoolName, true);

        triggerCalled = false;
    }


    // UpdateState gets called each frame and runs the logic of the current state
    public virtual void UpdateState() {

        stateTimer -= Time.deltaTime;

        // For updating the JumpFall blend tree based on the player's current vertical velocity
        anim.SetFloat(_yVelocityHash, rb.linearVelocityY);

        if (inputSet.Player.Dash.WasPressedThisFrame() && CanDash())
            stateMachine.ChangeState(player.DashState);
    }


    // ExitState of the current state gets called before transitioning to a new state.
    public virtual void ExitState() {
        anim.SetBool(animBoolName, false);
    }

    private bool CanDash() {
        if (player.WallDetected || stateMachine.CurrentState == player.DashState)
            return false;

        return true;
    }

    public void CallAnimationTrigger() {
        triggerCalled = true;
    }
}
