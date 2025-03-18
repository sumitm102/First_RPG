using UnityEngine;

public class PlayerStateMachine 
{
    public PlayerState currentState { get; private set; }

    public void Initialize(PlayerState _startState) {
        currentState = _startState;
        currentState?.EnterState();
        
    }

    public void ChangeState(PlayerState _newState) {
        currentState?.ExitState();
        currentState = _newState;
        currentState?.EnterState();
    }
}
