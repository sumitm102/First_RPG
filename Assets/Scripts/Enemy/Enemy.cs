using System.Collections;
using UnityEngine;

public class Enemy : Entity
{

    [field: Header("Movement details")]
    [field: SerializeField] public float MoveSpeed { get; private set; } = 1.4f;
    [field: SerializeField] public float IdleTime { get; private set; } = 2f;
    [field: SerializeField, Range(0, 2)] public float MoveAnimSpeedMultiplier { get; private set; } = 1f;


    [field: Header("Battle details")]
    [field: SerializeField] public float BattleMoveSpeed { get; private set; } = 3f;
    [field: SerializeField] public float AttackDistance { get; private set; } = 2f;
    [field: SerializeField] public float BattleTimeDuration { get; private set; } = 5f;
    [field: SerializeField] public float MinRetreatDistance { get; private set; } = 1f;
    [field: SerializeField] public Vector2 RetreatVelocity { get; private set; }


    [field: Header("Stunned state details")]
    [field: SerializeField] public float StunnedDuration { get; private set; } = 1f;
    [field: SerializeField] public Vector2 StunnedVelocity { get; private set; } = new Vector2(7f, 7f);
    [field:SerializeField] public bool CanBeStunned { get; private set; }


    [field:Header("Player detection")]
    [field:SerializeField] public Transform PlayerCheck { get; private set; } 
    [field:SerializeField] public LayerMask PlayerDetectionLayer { get; private set;}
    [field: SerializeField] public float PlayerCheckDistance { get; private set; } = 10f;

    public Transform PlayerTransform { get; private set; }

    private Coroutine _handlePlayerDeathCo;


    #region States

    public EnemyIdleState IdleState;
    public EnemyMoveState MoveState;
    public EnemyAttackState AttackState;
    public EnemyBattleState BattleState;
    public EnemyDeadState DeadState;
    public EnemyStunnedState StunnedState;

    #endregion


    protected override void Start() {
        base.Start();

        if(IdleState != null)
            StateMachine.Initialize(IdleState);
    }

    public Transform GetPlayerReference() {
        if (PlayerTransform == null)
            PlayerTransform = PlayerDetected().transform;

        return PlayerTransform;
    }


    public RaycastHit2D PlayerDetected() {

        RaycastHit2D hit =  Physics2D.Raycast(PlayerCheck.position, Vector3.right * FacingDir, PlayerCheckDistance, groundDetectionLayer | PlayerDetectionLayer);

        if (hit.collider == null || hit.collider.gameObject.layer != LayerMask.NameToLayer("Player"))
            return default;
        

        return hit;
    }

    public void TryEnterBattleState(Transform damageDealer) {

        // No need to implement the rest of the method if enemy is already in battle state or the attack state
        if (StateMachine.CurrentState == BattleState || StateMachine.CurrentState == AttackState)
            return;

        PlayerTransform = damageDealer;
        StateMachine.ChangeState(BattleState);
    }

    public override void TryEnterDeadState() {
        base.TryEnterDeadState();

        if(DeadState != null || StateMachine.CurrentState != DeadState)
            StateMachine.ChangeState(DeadState);
    }

    #region Handle Player Death

    private void HandlePlayerDeath() {
        if (_handlePlayerDeathCo != null)
            StopCoroutine(HandlePlayerDeathCo());

        _handlePlayerDeathCo = StartCoroutine(HandlePlayerDeathCo());  
    }

    private IEnumerator HandlePlayerDeathCo() {
        yield return new WaitForSeconds(0.3f);
        StateMachine.ChangeState(IdleState);
    }

    #endregion


    public void EnableCounterWindow(bool enable) => CanBeStunned = enable;

    protected override IEnumerator SlowDownEntityCo(float duration, float slowMultiplier) {

        float originalMoveSpeed = MoveSpeed;
        float originalBattleMoveSpeed = BattleMoveSpeed;
        float originalAnimSpeed = Anim.speed;

        float speedMultiplier = 1f - slowMultiplier;
        MoveSpeed *= speedMultiplier;
        BattleMoveSpeed *= speedMultiplier;
        Anim.speed *= speedMultiplier;

        yield return new WaitForSeconds(duration);

        MoveSpeed = originalMoveSpeed;
        BattleMoveSpeed = originalBattleMoveSpeed;
        Anim.speed = originalAnimSpeed;
    }


    private void OnEnable() {
            Player.onPlayerDeath += HandlePlayerDeath;
    }

    private void OnDisable() {
            Player.onPlayerDeath -= HandlePlayerDeath;
    }

    protected override void OnDrawGizmos() {
        base.OnDrawGizmos();

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(PlayerCheck.position, new Vector3(PlayerCheck.position.x + (FacingDir * PlayerCheckDistance), PlayerCheck.position.y));

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(PlayerCheck.position, new Vector3(PlayerCheck.position.x + (FacingDir * AttackDistance), PlayerCheck.position.y));

        Gizmos.color = Color.green;
        Gizmos.DrawLine(PlayerCheck.position, new Vector3(PlayerCheck.position.x + (FacingDir * MinRetreatDistance), PlayerCheck.position.y));
    }

}
