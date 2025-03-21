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

    public int facingDir { get; private set; } = 1;
    protected bool isFacingRight = true;

    #region Components
    [field: SerializeField] public Animator animator { get; private set; }
    [field: SerializeField] public Rigidbody2D rbody { get; private set; }

    #endregion

    protected virtual void Awake() {

    }

    protected virtual void Start() {
        if (animator == null) animator = GetComponentInChildren<Animator>();
        if (rbody == null) rbody = GetComponent<Rigidbody2D>();
    }

    protected virtual void Update() {

    }

    public void SetVelocity(float _xVelocity, float _yVelocity) {

        rbody.linearVelocity = new Vector2(_xVelocity, _yVelocity);
        FlipController(_xVelocity);
    }

    #region Collision Detection
    public virtual bool IsGroundDetected() => Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, groundLayer);
    public virtual bool IsWallDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, wallCheckDistance, groundLayer);

    protected virtual void OnDrawGizmos() {
        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance, 0f));
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y, 0f));
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
}
