using UnityEngine;

public abstract class EntityState
{
    protected StateMachine stateMachine;
    protected int animBoolName;

    protected Animator anim;
    protected Rigidbody2D rb;

    protected float stateTimer;
    protected bool triggerCalled;


    public EntityState(StateMachine sm, int abn) {
        stateMachine = sm;
        animBoolName = abn;

    }


    // EnterState gets called at first after transitioning to a new state.
    public virtual void EnterState() {
        anim.SetBool(animBoolName, true);
        triggerCalled = false;
    }


    // UpdateState gets called each frame and runs the logic of the current state
    public virtual void UpdateState() {

        stateTimer -= Time.deltaTime;

        UpdateAnimationParameter();
    }


    // ExitState of the current state gets called before transitioning to a new state.
    public virtual void ExitState() {
        anim.SetBool(animBoolName, false);
    }


    public void AnimationTrigger() {
        triggerCalled = true;
    }

    public virtual void UpdateAnimationParameter() {

    }
}
