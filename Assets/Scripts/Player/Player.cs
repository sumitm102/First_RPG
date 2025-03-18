using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerStateMachine playerStateMachine { get; private set; }
    public PlayerIdleState playerIdleState { get; private set; }
    public PlayerMoveState playerMoveState { get; private set; }

    public void Awake() {

        //Initializing state machine and states
        playerStateMachine = new PlayerStateMachine();
        playerIdleState = new PlayerIdleState(this, playerStateMachine, "Idle");
        playerMoveState = new PlayerMoveState(this, playerStateMachine, "Move");
    }

    public void Start() {

        //Game starts with the idle state
        playerStateMachine.Initialize(playerIdleState);
    }

    public void Update() {

        playerStateMachine.currentState.UpdateState();
    }
}
