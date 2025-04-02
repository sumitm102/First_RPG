using System.Collections;
using UnityEngine;

public class Enemy : Entity
{
    [SerializeField] protected LayerMask playerLayer;

    [Header("Stunned info")]
    public float stunDuration;
    public Vector2 stunVelocity;
    protected bool canBeStunned;
    [SerializeField] protected GameObject counterImage;

    [Header("Move info")]
    public float moveSpeed;
    public float idleTime;
    public float battleTime;
    private float _defaultMoveSpeed;

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

        _defaultMoveSpeed = moveSpeed;
    }


    protected override void Update() {
        base.Update();

        enemyStateMachine.currentState.UpdateState();
        //Debug.Log(IsPlayerDetected().collider.gameObject.name);
    }


    public virtual void FreezeTime(bool _timeFrozen) {

        //Stopping move speed and animation to implement immobile enemies
        if (_timeFrozen) {
            moveSpeed = 0;
            animator.speed = 0;
        }
        else {
            moveSpeed = _defaultMoveSpeed;
            animator.speed = 1;
        }
    }

    protected virtual IEnumerator FreezeTimeFor(float _seconds) {
        FreezeTime(true);

        yield return new WaitForSeconds(_seconds);

        FreezeTime(false);

    }

    #region Counter Attack Window
    public virtual void OpenCounterAttackWindow() {
        canBeStunned = true;
        counterImage?.SetActive(true);
    }

    public virtual void CloseCounterAttackWindow() {
        canBeStunned = false;
        counterImage?.SetActive(false);
    }

    #endregion

    public virtual bool CanBeStunned() {

        //Enemy will be stunned only once and not more
        if (canBeStunned) {
            CloseCounterAttackWindow();
            return true;
        }

        return false;
    }

    public virtual RaycastHit2D IsPlayerDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, detectionRange, playerLayer);
    public virtual void AnimationFinishTrigger() => enemyStateMachine.currentState.AnimationFinishTrigger();

    protected override void OnDrawGizmos() {
        base.OnDrawGizmos();

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x + attackDistance * facingDir, transform.position.y));
    }

}
