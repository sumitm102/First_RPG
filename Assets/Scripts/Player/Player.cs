using UnityEngine;

public class Player : MonoBehaviour
{
    private PlayerInputSet _inputSet;
    public StateMachine PlayerStateMachine { get; private set; }

    #region States

    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }

    #endregion

    [field: SerializeField] public Vector2 MoveInput { get; private set; }

    private void Awake() {
        PlayerStateMachine = new StateMachine();
        _inputSet = new PlayerInputSet();

        IdleState = new PlayerIdleState(PlayerStateMachine, "IdleState", this);
        MoveState = new PlayerMoveState(PlayerStateMachine, "MoveState", this);
    }

    private void OnEnable() {
        _inputSet.Enable();

        _inputSet.Player.Movement.performed += ctx => MoveInput = ctx.ReadValue<Vector2>();
        _inputSet.Player.Movement.canceled += ctx => MoveInput = Vector2.zero;
    }

    private void OnDisable() {
        _inputSet.Disable();
    }

    private void Start() {
        PlayerStateMachine.Initialize(IdleState);
    }

    private void Update() {
        PlayerStateMachine.UpdateActiveState();
    }
}
