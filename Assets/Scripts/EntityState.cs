using UnityEngine;

public abstract class EntityState 
{
    protected Player player;
    protected StateMachine stateMachine;
    protected int animBoolName;

    protected Animator anim;
    protected Rigidbody2D rb;
    protected PlayerInputSet inputSet;

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
    }


    // UpdateState gets called each frame and runs the logic of the current state
    public virtual void UpdateState() {

        // For updating the JumpFall blend tree based on the player's current vertical velocity
        anim.SetFloat(_yVelocityHash, rb.linearVelocityY);
    }


    // ExitState of the current state gets called before transitioning to a new state.
    public virtual void ExitState() {
        anim.SetBool(animBoolName, false);
    }
}
