using UnityEngine;

public class Player : MonoBehaviour
{
    private PlayerInputSet _inputSet;
    public StateMachine PlayerStateMachine { get; private set; }

    #region States

    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }

    #endregion

    #region Anim Bools

    private readonly int _idleHash = Animator.StringToHash("Idle");
    private readonly int _moveHash = Animator.StringToHash("Move");

    #endregion

    #region Components

    [field: SerializeField] public Animator Anim { get; private set; }
    [field: SerializeField] public Rigidbody2D RB { get; private set; }

    #endregion

    [field: SerializeField] public Vector2 MoveInput { get; private set; }

    private void Awake() {

        if(Anim == null) Anim = GetComponentInChildren<Animator>();
        if(RB == null) RB = GetComponent<Rigidbody2D>();

        PlayerStateMachine = new StateMachine();
        _inputSet = new PlayerInputSet();

        IdleState = new PlayerIdleState(PlayerStateMachine, _idleHash, this);
        MoveState = new PlayerMoveState(PlayerStateMachine, _moveHash, this);
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
