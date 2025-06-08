using UnityEngine;

public abstract class EntityState 
{
    protected Player player;
    protected StateMachine stateMachine;
    protected int animBoolName;

    protected Animator anim;

    public EntityState(StateMachine sm, int abn, Player p) {
        stateMachine = sm;
        animBoolName = abn;
        player = p;

        anim = player.Anim;
    }


    // EnterState gets called at first after transitioning to a new state.
    public virtual void EnterState() {
        anim.SetBool(animBoolName, true);
    }


    // UpdateState gets called each frame and runs the logic of the current state
    public virtual void UpdateState() {
        //Debug.Log("Updating " + stateName);
    }


    // ExitState of the current state gets called before transitioning to a new state.
    public virtual void ExitState() {
        anim.SetBool(animBoolName, false);
    }
}
