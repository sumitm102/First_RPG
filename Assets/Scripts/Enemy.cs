using UnityEngine;

public class Enemy : Entity
{
    [SerializeField] protected LayerMask playerLayer;

    [Header("Move info")]
    public float moveSpeed;
    public float idleTime;
    public float battleTime;

    [Header("Attack info")]
    public float attackDistance;
    public float attackCooldown;
    public float detectionRange;
    [HideInInspector] public float lastTimeAttacked;

    public EnemyStateMachine enemyStateMachine { get; private set; }



    protected override void Awake() {
        base.Awake();

        enemyStateMachine = new EnemyStateMachine();
    }

    protected override void Start() {
        base.Start();
    }


    protected override void Update() {
        base.Update();

        enemyStateMachine.currentState.UpdateState();
        //Debug.Log(IsPlayerDetected().collider.gameObject.name);
    }


    public virtual RaycastHit2D IsPlayerDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, detectionRange, playerLayer);

    protected override void OnDrawGizmos() {
        base.OnDrawGizmos();

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x + attackDistance * facingDir, transform.position.y));
    }

    public virtual void AnimationFinishTrigger() => enemyStateMachine.currentState.AnimationFinishTrigger();
}
