using UnityEngine;

public class Player : MonoBehaviour
{
    #region States
    public PlayerStateMachine playerStateMachine { get; private set; }
    public PlayerIdleState playerIdleState { get; private set; }
    public PlayerMoveState playerMoveState { get; private set; }

    #endregion

    #region Components
    [field:SerializeField] public Animator playerAnimator { get; private set; }

    #endregion

    public void Awake() {

        //Initializing state machine and states
        playerStateMachine = new PlayerStateMachine();
        playerIdleState = new PlayerIdleState(this, playerStateMachine, "Idle");
        playerMoveState = new PlayerMoveState(this, playerStateMachine, "Move");
    }

    public void Start() {

        if (playerAnimator == null) playerAnimator = GetComponentInChildren<Animator>();

        //Game starts with the idle state
        playerStateMachine.Initialize(playerIdleState);
    }

    public void Update() {

        playerStateMachine.currentState.UpdateState();
    }
}
