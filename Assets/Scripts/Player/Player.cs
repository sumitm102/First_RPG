using UnityEngine;

public class Player : MonoBehaviour {
    private PlayerInputSet _inputSet;
    public StateMachine PlayerStateMachine { get; private set; }


    [field: Header("Movement details")]
    [field: SerializeField] public float MoveSpeed { get; private set; } = 8f;


    #region States

    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }

    #endregion


    #region Anim Bools

    private static readonly int _idleHash = Animator.StringToHash("Idle");
    private static readonly int _moveHash = Animator.StringToHash("Move");

    #endregion


    #region Components

    [field: SerializeField] public Animator Anim { get; private set; }
    [field: SerializeField] public Rigidbody2D RB { get; private set; }

    #endregion

    private bool _isFacingRight = true;

    public Vector2 MoveInput { get; private set; }


    private void Awake() {

        if (Anim == null) Anim = GetComponentInChildren<Animator>();
        if (RB == null) RB = GetComponent<Rigidbody2D>();

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

    public void SetVelocity(float xVelocity, float yVelocity) {
        RB.linearVelocity = new Vector2(xVelocity, yVelocity);
        HandleFlip(xVelocity);
    }

    private void HandleFlip(float xVelocity) {

        // Flip character if horizontal velocity is towards the right but player is facing left or  if horizontal velocity is towards the left but the player is facing right
        if ((xVelocity > 0 && !_isFacingRight) || (xVelocity < 0 && _isFacingRight))
            FlipCharacter();


        // Also works
        // Flip character if input is towards the right but player is facing left or  if input is towards the left but the player is facing right
        //if ((MoveInput.x > 0 && !_isFacingRight) || (MoveInput.x < 0 && _isFacingRight))
        //    FlipCharacter();
    }

    private void FlipCharacter() {
        transform.Rotate(0, 180, 0);
        _isFacingRight = !_isFacingRight;
    }
}
