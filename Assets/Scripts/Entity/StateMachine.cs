using UnityEngine;

public class StateMachine
{
    public EntityState CurrentState { get; private set; }
    private bool _canEnterNewState = true;

    public void Initialize(EntityState startState) {
        _canEnterNewState = true;
        CurrentState = startState;
        CurrentState?.EnterState();
    }

    public void ChangeState(EntityState newState) {

        if (!_canEnterNewState)
            return;

        CurrentState?.ExitState();
        CurrentState = newState;
        CurrentState?.EnterState();
    }

    public void UpdateActiveState() {
        CurrentState.UpdateState();
    }

    public void PreventChangingToNewState() => _canEnterNewState = false;
    
}
