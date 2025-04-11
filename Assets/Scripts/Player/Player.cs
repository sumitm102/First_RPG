using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Player : Entity
{
    #region PlayerStats
    [Header("Move stats")]   
    public float moveSpeed = 12f;
    public float jumpForce = 12f;
    public float swordReturnImpact = 8f;

    [Header("Dash info")]
    public float dashSpeed = 25f;
    public float dashDuration = 0.3f;
    //[SerializeField] private float _dashCooldownAmount = 3f;
    //private float _dashCooldownTimer;
    public float dashDir { get; private set; }

    [Header("Attack Details")]
    public Vector2[] attackMovement;
    public float counterAttackDuration = 0.2f;

    [Header("Jump Buffering")]
    public float jumpBufferTime;

    #endregion

    public bool isPerformingAction { get; private set; }
    public SkillManager skillManager { get; private set; }
    public GameObject sword { get; private set; }


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
    public PlayerCounterAttackState playerCounterAttackState { get; private set; }
    public PlayerAimSwordState playerAimSwordState { get; private set; }
    public PlayerCatchSwordState playerCatchSwordState { get; private set; }
    public PlayerBlackholeAbilityState playerBlackholeState { get; private set; }

    #endregion


    protected override void Awake() {
        base.Awake();

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
        playerCounterAttackState = new PlayerCounterAttackState(this, playerStateMachine, "CounterAttack");

        playerAimSwordState = new PlayerAimSwordState(this, playerStateMachine, "AimSword");
        playerCatchSwordState = new PlayerCatchSwordState(this, playerStateMachine, "CatchSword");

        playerBlackholeState = new PlayerBlackholeAbilityState(this, playerStateMachine, "Jump");
    }

    protected override void Start() {
        base.Start();

        skillManager = SkillManager.Instance;

        //Game starts with the idle state
        playerStateMachine.Initialize(playerIdleState);
    }


    protected override void Update() {
        base.Update();

        playerStateMachine.currentState.UpdateState();

        //Checking for dash needs to be outside of grounded to be applicable almost everywhere for use
        CheckForDashInput();

        // Teleports to the crystal's position if g is pressed and other requirements are fulfilled
        if (Input.GetKeyDown(KeyCode.G))
            skillManager.crystalSkill.CanUseSkill();
    }

    public IEnumerator BusyFor(float _seconds) {
        isPerformingAction = true;

        yield return new WaitForSeconds(_seconds);

        isPerformingAction = false;
    }

    public void AnimationTrigger() => playerStateMachine.currentState.AnimationFinishTrigger();
    private void CheckForDashInput() {

        //To make sure to not dash when wall sliding
        if (IsWallDetected()) return;

        if (Input.GetKeyDown(KeyCode.LeftShift) && SkillManager.Instance.dashSkill.CanUseSkill()) {

            dashDir = Input.GetAxisRaw("Horizontal");

            if (dashDir == 0) dashDir = facingDir;

            playerStateMachine.ChangeState(playerDashState);
        }
    }


    #region Sword
    public void AssignNewSword(GameObject _newSword) {
        sword = _newSword;
    }

    public void CatchSword() {
        playerStateMachine.ChangeState(playerCatchSwordState);

        Destroy(sword);
    }

    #endregion


}
