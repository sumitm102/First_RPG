using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour {
    public PlayerInputSet InputSet { get; private set; }
    public StateMachine PlayerStateMachine { get; private set; }


    [field: Header("Movement details")]
    [field: SerializeField] public float MoveSpeed { get; private set; } = 8f;
    [field: SerializeField] public float JumpForce { get; private set; } = 12f;
    [field: SerializeField, Range(0, 1)] public float InAirMultiplier { get; private set; } = 0.65f; // Range should be from 0 to 1
    [field: SerializeField, Range(0, 1)] public float SlowedWallSlideMultiplier { get; private set; } = 0.3f; // Range should be from 0 to 1
    [field: SerializeField] public Vector2 WallJumpForce { get; private set; }
    [field: Space]
    [field: SerializeField] public float DashDuration { get; private set; } = 0.25f;
    [field: SerializeField] public float DashSpeed { get; private set; } = 20f;
    [field: SerializeField] public float DashCooldown { get; private set; }

    [field: Header("Attack details")]
    [field: SerializeField] public Vector2[] AttackVelocity { get; private set; }
    [field: SerializeField] public float AttackVelocityDuration { get; private set; } = 0.1f;
    [field: SerializeField] public float ComboResetTime { get; private set; } = 1f;
    private Coroutine _queuedAttackCo;

    [Header("Collision Detection")]
    [SerializeField] private Transform _groundCheckTransform;
    [SerializeField] private float _groundCheckDistance;
    [SerializeField] private LayerMask _detectionLayer;
    [SerializeField] private float _wallCheckDistance;
    public bool GroundDetected { get; private set; }
    public bool WallDetected { get; private set; }
    

    #region States

    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }
    public PlayerJumpState JumpState { get; private set; }
    public PlayerFallState FallState { get; private set; }
    public PlayerWallSlideState WallSlideState { get; private set; }
    public PlayerWallJumpState WallJumpState { get; private set; }
    public PlayerDashState DashState { get; private set; }
    public PlayerBasicAttackState BasicAttackState { get; private set; }

    #endregion


    #region Anim Bools

    private static readonly int _idleHash = Animator.StringToHash("Idle");
    private static readonly int _moveHash = Animator.StringToHash("Move");
    private static readonly int _jumpFallHash = Animator.StringToHash("JumpFall"); // Same hash for two states since they both need to enter the blend tree
    private static readonly int _wallSlideHash = Animator.StringToHash("WallSlide");
    private static readonly int _dashHash = Animator.StringToHash("Dash");
    private static readonly int _basicAttackHash = Animator.StringToHash("BasicAttack");


    #endregion


    #region Components

    [field: SerializeField] public Animator Anim { get; private set; }
    [field: SerializeField] public Rigidbody2D RB { get; private set; }

    #endregion

    private bool _isFacingRight = true;
    public int FacingDir { get; private set; } = 1;

    public Vector2 MoveInput { get; private set; }


    private void Awake() {

        if (Anim == null) Anim = GetComponentInChildren<Animator>();
        if (RB == null) RB = GetComponent<Rigidbody2D>();

        PlayerStateMachine = new StateMachine();
        InputSet = new PlayerInputSet(); // Input needs to be initialized before the states

        #region State Initialization

        IdleState = new PlayerIdleState(PlayerStateMachine, _idleHash, this);
        MoveState = new PlayerMoveState(PlayerStateMachine, _moveHash, this);
        JumpState = new PlayerJumpState(PlayerStateMachine, _jumpFallHash, this);
        FallState = new PlayerFallState(PlayerStateMachine, _jumpFallHash, this);
        WallSlideState = new PlayerWallSlideState(PlayerStateMachine, _wallSlideHash, this);
        WallJumpState = new PlayerWallJumpState(PlayerStateMachine, _jumpFallHash, this);
        DashState = new PlayerDashState(PlayerStateMachine, _dashHash, this);
        BasicAttackState = new PlayerBasicAttackState(PlayerStateMachine, _basicAttackHash, this);

        #endregion
    }

    private void OnEnable() {
        InputSet.Enable();

        InputSet.Player.Movement.performed += ctx => MoveInput = ctx.ReadValue<Vector2>();
        InputSet.Player.Movement.canceled += ctx => MoveInput = Vector2.zero;
    }

    private void OnDisable() {
        InputSet.Disable();
    }

    private void Start() {
        PlayerStateMachine.Initialize(IdleState);
    }

    private void Update() {
        HandleCollisionDetection();
        PlayerStateMachine.UpdateActiveState();
    }

    public void SetVelocity(float xVelocity, float yVelocity) {
        RB.linearVelocity = new Vector2(xVelocity, yVelocity);
        HandleFlip(xVelocity);
    }

    public void CallAnimationTrigger() {
        PlayerStateMachine.CurrentState.CallAnimationTrigger();
    }

    private void HandleFlip(float xVelocity) {

        // Flip character if horizontal velocity is towards the right but player is facing left or  if horizontal velocity is towards the left but the player is facing right
        if ((xVelocity > 0 && !_isFacingRight) || (xVelocity < 0 && _isFacingRight))
            FlipCharacter();
    }

    private void FlipCharacter() {
        transform.Rotate(0, 180, 0);
        _isFacingRight = !_isFacingRight;
        FacingDir = -FacingDir;
    }

    private void HandleCollisionDetection() {
        GroundDetected = Physics2D.Raycast(_groundCheckTransform.position, Vector3.down, _groundCheckDistance, _detectionLayer);
        WallDetected = Physics2D.Raycast(transform.position, Vector3.right * FacingDir, _wallCheckDistance, _detectionLayer);
    }

    public void EnterAttackStateWithDelay() {

        // To stop running multiple coroutines at the same time
        if (_queuedAttackCo != null)
            StopCoroutine(_queuedAttackCo);

        _queuedAttackCo = StartCoroutine(EnterAttackStateWithDelayCo());
    }
    private IEnumerator EnterAttackStateWithDelayCo() {
        yield return new WaitForEndOfFrame();
        PlayerStateMachine.ChangeState(BasicAttackState);
    }

    private void OnDrawGizmos() {
        Gizmos.DrawLine(_groundCheckTransform.position, _groundCheckTransform.position + new Vector3(0, -_groundCheckDistance));
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(FacingDir * _wallCheckDistance, 0));
    }
}
