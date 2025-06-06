using UnityEngine;

public class PlayerStateMachine 
{
    public PlayerState currentState { get; private set; }
    public PlayerState previousState { get; private set; }   

    public void Initialize(PlayerState _startState) {
        previousState = _startState;

        currentState = _startState;
        currentState?.EnterState();
        
    }

    public void ChangeState(PlayerState _newState) {
        previousState = currentState;

        currentState?.ExitState();
        currentState = _newState;
        currentState?.EnterState();
    }
}
