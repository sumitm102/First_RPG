using UnityEngine;

public abstract class EntityState 
{
    protected Player player;
    protected StateMachine stateMachine;
    protected string stateName;

    public EntityState(StateMachine sm, string sn, Player p) {
        stateMachine = sm;
        stateName = sn;
        player = p;
    }


    // EnterState gets called at first after transitioning to a new state.
    public virtual void EnterState() {
        //Debug.Log("Entering " + stateName);
    }


    // UpdateState gets called each frame and runs the logic of the current state
    public virtual void UpdateState() {
        //Debug.Log("Updating " + stateName);
    }


    // ExitState of the current state gets called before transitioning to a new state.
    public virtual void ExitState() {
        //Debug.Log("Exiting " + stateName);
    }
}
