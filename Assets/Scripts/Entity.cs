using System.Collections;
using UnityEngine;

public class Entity : MonoBehaviour
{

    [Header("Collision info")]
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected float groundCheckDistance;
    [SerializeField] protected Transform wallCheck;
    [SerializeField] protected float wallCheckDistance;
    [SerializeField] protected LayerMask groundLayer;
    [SerializeField] protected LayerMask wallLayer;

    public Transform attackCheck;
    public float attackCheckRadius;

    [Header("Knockback info")]
    [SerializeField] protected Vector2 knockBackVelocity;
    [SerializeField] protected float knockBackDuration;
    protected bool isKnockedBack;


    public int facingDir { get; private set; } = 1;
    protected bool isFacingRight = true;

    #region Components
    [field: SerializeField] public Animator animator { get; private set; }
    [field: SerializeField] public Rigidbody2D rbody { get; private set; }
    [field: SerializeField] public EntityFX fx { get; private set; }
    [field: SerializeField] public SpriteRenderer spriteRenderer { get; private set; }

    #endregion

    protected virtual void Awake() {

    }

    protected virtual void Start() {
        if (animator == null) animator = GetComponentInChildren<Animator>();
        if (rbody == null) rbody = GetComponent<Rigidbody2D>();
        if (fx == null) fx = GetComponent<EntityFX>();
        if (spriteRenderer == null) spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    protected virtual void Update() {

    }

    public virtual void Damage() {
       
        fx.StartCoroutine("FlashFX");
        StartCoroutine("Knockback");
    }

    protected virtual IEnumerator Knockback() {
        isKnockedBack = true;
        rbody.linearVelocity = new Vector2(knockBackVelocity.x * -facingDir, knockBackVelocity.y);

        yield return new WaitForSeconds(knockBackDuration);

        isKnockedBack = false;
    }


    #region Velocity
    public void SetVelocity(float _xVelocity, float _yVelocity) {

        //To avoid moving when knocked back
        if (isKnockedBack) return;

        rbody.linearVelocity = new Vector2(_xVelocity, _yVelocity);
        FlipController(_xVelocity);
    }

    public void SetZeroVelocity() {
        if (isKnockedBack) return;

        rbody.linearVelocity = new Vector2(0, 0);
    }

    #endregion

    #region Collision Detection
    public virtual bool IsGroundDetected() => Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, groundLayer);
    public virtual bool IsWallDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, wallCheckDistance, groundLayer);

    protected virtual void OnDrawGizmos() {
        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance, 0f));
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y, 0f));
        Gizmos.DrawWireSphere(attackCheck.position, attackCheckRadius);
    }

    #endregion

    #region Flipping Character
    public virtual void FlipCharacter() {
        facingDir *= -1;
        isFacingRight = !isFacingRight;
        transform.Rotate(0, 180, 0);
    }

    public virtual void FlipController(float _horizontalMovemnt) {
        //If player is moving left but not facing left, flip the player
        if (_horizontalMovemnt < 0 && isFacingRight) FlipCharacter();
        //If player is moving right but not facing right, flip the player
        else if (_horizontalMovemnt > 0 && !isFacingRight) FlipCharacter();
    }
    #endregion

    public void MakeTransparent(bool _isTransparent) {
        if (_isTransparent) 
            spriteRenderer.color = Color.clear;
        else
            spriteRenderer.color = Color.white;
    }
}
