using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region PlayerStats
    [Header("Move stats")]   
    public float moveSpeed = 12f;
    public float jumpForce = 12f;

    [Header("Dash info")]
    public float dashSpeed = 25f;
    public float dashDuration = 0.3f;
    [SerializeField] private float _dashCooldownAmount = 3f;
    private float _dashCooldownTimer;
    public float dashDir { get; private set; }

    #endregion

    public bool isPerformingAction { get; private set; }


    [Header("Collision info")]
    [SerializeField] private Transform _groundCheck;
    [SerializeField] private float _groundCheckDistance;
    [SerializeField] private Transform _wallCheck;
    [SerializeField] private float _wallCheckDistance;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private LayerMask _wallLayer;

    public int facingDir { get; private set; } = 1;
    private bool isFacingRight = true;

    #region States
    public PlayerStateMachine playerStateMachine { get; private set; }
    public PlayerIdleState playerIdleState { get; private set; }
    public PlayerMoveState playerMoveState { get; private set; }
    public PlayerJumpState playerJumpState { get; private set; }
    public PlayerAirState playerAirState { get; private set; }
    public PlayerDashState playerDashState { get; private set; }
    public PlayerWallSlideState playerWallSliderState { get; private set; }
    public PlayerWallJumpState playerWallJumpState { get; private set; }
    public PlayerPrimaryAttackState playerPrimaryAttackState { get; private set; }

    #endregion

    #region Components
    [field:SerializeField] public Animator playerAnimator { get; private set; }
    [field: SerializeField] public Rigidbody2D playerRigidbody { get; private set; }

    #endregion

    public void Awake() {

        //Initializing state machine and states
        playerStateMachine = new PlayerStateMachine();
        playerIdleState = new PlayerIdleState(this, playerStateMachine, "Idle");
        playerMoveState = new PlayerMoveState(this, playerStateMachine, "Move");
        playerJumpState = new PlayerJumpState(this, playerStateMachine, "Jump");
        playerAirState = new PlayerAirState(this, playerStateMachine, "Jump");
        playerDashState = new PlayerDashState(this, playerStateMachine, "Dash");
        playerWallSliderState = new PlayerWallSlideState(this, playerStateMachine, "WallSlide");
        playerWallJumpState = new PlayerWallJumpState(this, playerStateMachine, "Jump");
        playerPrimaryAttackState = new PlayerPrimaryAttackState(this, playerStateMachine, "Attack");
    }

    public void Start() {

        if (playerAnimator == null) playerAnimator = GetComponentInChildren<Animator>();
        if (playerRigidbody == null) playerRigidbody = GetComponent<Rigidbody2D>();

        //Game starts with the idle state
        playerStateMachine.Initialize(playerIdleState);
    }


    public void Update() {

        playerStateMachine.currentState.UpdateState();

        //Checking for dash needs to be outside of grounded to be applicable almost everywhere for use
        CheckForDashInput();
    }

    public IEnumerator BusyFor(float _seconds) {
        isPerformingAction = true;

        yield return new WaitForSeconds(_seconds);

        isPerformingAction = false;
    }

    public void SetVelocity(float _xVelocity, float _yVelocity) {

        playerRigidbody.linearVelocity = new Vector2(_xVelocity, _yVelocity);
        FlipController(_xVelocity);
    }

    public bool IsGroundDetected() => Physics2D.Raycast(_groundCheck.position, Vector2.down, _groundCheckDistance, _groundLayer);
    public bool IsWallDetected() => Physics2D.Raycast(_wallCheck.position, Vector2.right * facingDir, _wallCheckDistance, _groundLayer);

    public void AnimationTrigger() => playerStateMachine.currentState.AnimationFinishTrigger();
    private void CheckForDashInput() {

        _dashCooldownTimer -= Time.deltaTime;

        //To make sure to not dash when wall sliding while decreasing the cool down timer above
        if (IsWallDetected()) return;

        if (_dashCooldownTimer < 0f && Input.GetKeyDown(KeyCode.LeftShift)) {
            _dashCooldownTimer = _dashCooldownAmount;

            dashDir = Input.GetAxisRaw("Horizontal");

            if (dashDir == 0) dashDir = facingDir;

            playerStateMachine.ChangeState(playerDashState);
        }
    }

    public void FlipPlayer() {
        facingDir *= -1;
        isFacingRight = !isFacingRight;
        transform.Rotate(0, 180, 0);
    }

    public void FlipController(float _horizontalMovemnt) {
        //If player is moving left but not facing left, flip the player
        if (_horizontalMovemnt < 0 && isFacingRight) FlipPlayer();
        //If player is moving right but not facing right, flip the player
        else if (_horizontalMovemnt > 0 && !isFacingRight) FlipPlayer();
    }

    private void OnDrawGizmos() {
        Gizmos.DrawLine(_groundCheck.position, new Vector3(_groundCheck.position.x, _groundCheck.position.y - _groundCheckDistance, 0f));
        Gizmos.DrawLine(_wallCheck.position, new Vector3(_wallCheck.position.x + _wallCheckDistance, _wallCheck.position.y, 0f));
    }
}
