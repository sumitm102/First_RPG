using UnityEngine;

public class StateMachine
{
    public EntityState CurrentState { get; private set; }

    public void Initialize(EntityState startState) {
        CurrentState = startState;
        CurrentState?.EnterState();
    }

    public void ChangeState(EntityState newState) {
        CurrentState?.ExitState();
        CurrentState = newState;
        CurrentState?.EnterState();
    }

    public void UpdateActiveState() {
        CurrentState.UpdateState();
    }
}
