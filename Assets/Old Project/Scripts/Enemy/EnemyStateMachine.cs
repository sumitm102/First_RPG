using UnityEngine;

public class EnemyStateMachine 
{
    public EnemyState currentState { get; private set; }
    public Enemy enemy;

    public void Initialize(EnemyState _startState) {
        currentState = _startState;
        currentState?.EnterState();
    }

    public void ChangeState(EnemyState _newState) {
        currentState?.ExitState();
        currentState = _newState;
        currentState?.EnterState();
    }

}
