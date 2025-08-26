using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Entity {
    public PlayerInputSet InputSet { get; private set; }


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
    [field: SerializeField] public Vector2 JumpAttackVelocity { get; private set; }
    [field: SerializeField] public float AttackVelocityDuration { get; private set; } = 0.1f;
    [field: SerializeField] public float ComboResetTime { get; private set; } = 1f;
    private Coroutine _queuedAttackCo;


    #region States

    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }
    public PlayerJumpState JumpState { get; private set; }
    public PlayerFallState FallState { get; private set; }
    public PlayerWallSlideState WallSlideState { get; private set; }
    public PlayerWallJumpState WallJumpState { get; private set; }
    public PlayerDashState DashState { get; private set; }
    public PlayerBasicAttackState BasicAttackState { get; private set; }
    public PlayerJumpAttackState JumpAttackState { get; private set; }
    public PlayerDeadState DeadState { get; private set; }
    public PlayerCounterAttackState CounterAttackState { get; private set; }

    #endregion

    #region Anim Bools

    private static readonly int _idleHash = Animator.StringToHash("Idle");
    private static readonly int _moveHash = Animator.StringToHash("Move");
    private static readonly int _jumpFallHash = Animator.StringToHash("JumpFall"); // Same hash for two states since they both need to enter the blend tree
    private static readonly int _wallSlideHash = Animator.StringToHash("WallSlide");
    private static readonly int _dashHash = Animator.StringToHash("Dash");
    private static readonly int _basicAttackHash = Animator.StringToHash("BasicAttack");
    private static readonly int _jumpAttackHash = Animator.StringToHash("JumpAttack");
    private static readonly int _deadHash = Animator.StringToHash("Dead");
    private static readonly int _counterAttackHash = Animator.StringToHash("CounterAttack");


    #endregion

    public Vector2 MoveInput { get; private set; }

    #region Action Events

    public static event Action onPlayerDeath;

    #endregion


    protected override void Awake() {
        base.Awake();

        InputSet = new PlayerInputSet(); // Input needs to be initialized before the states

        #region State Initialization

        IdleState = new PlayerIdleState(StateMachine, _idleHash, this);
        MoveState = new PlayerMoveState(StateMachine, _moveHash, this);
        JumpState = new PlayerJumpState(StateMachine, _jumpFallHash, this);
        FallState = new PlayerFallState(StateMachine, _jumpFallHash, this);
        WallSlideState = new PlayerWallSlideState(StateMachine, _wallSlideHash, this);
        WallJumpState = new PlayerWallJumpState(StateMachine, _jumpFallHash, this);
        DashState = new PlayerDashState(StateMachine, _dashHash, this);
        BasicAttackState = new PlayerBasicAttackState(StateMachine, _basicAttackHash, this);
        JumpAttackState = new PlayerJumpAttackState(StateMachine, _jumpAttackHash, this);
        DeadState = new PlayerDeadState(StateMachine, _deadHash, this);
        CounterAttackState = new PlayerCounterAttackState(StateMachine, _counterAttackHash, this);

        #endregion
    }

    #region Input

    private void OnEnable() {
        InputSet.Enable();

        InputSet.Player.Movement.performed += ctx => MoveInput = ctx.ReadValue<Vector2>();
        InputSet.Player.Movement.canceled += ctx => MoveInput = Vector2.zero;
    }

    private void OnDisable() {
        InputSet.Disable();
    }

    #endregion
    
    protected override void Start() {
        base.Start();

        StateMachine.Initialize(IdleState);
    }

    public void EnterAttackStateWithDelay() {

        // To stop running multiple coroutines at the same time
        if (_queuedAttackCo != null)
            StopCoroutine(_queuedAttackCo);

        _queuedAttackCo = StartCoroutine(EnterAttackStateWithDelayCo());
    }
    private IEnumerator EnterAttackStateWithDelayCo() {
        yield return new WaitForEndOfFrame();
        StateMachine.ChangeState(BasicAttackState);
    }

    public override void TryEnterDeadState() {
        base.TryEnterDeadState();

        if (DeadState != null || StateMachine.CurrentState != DeadState) {
            onPlayerDeath?.Invoke();
            StateMachine.ChangeState(DeadState);
        }
    }

}
