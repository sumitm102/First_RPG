using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.InputSystem;

public class Entity : MonoBehaviour
{
    protected StateMachine StateMachine { get; private set; }

    [Header("Collision Detection")]
    [SerializeField] private Transform _groundCheckTransform;
    [SerializeField] private float _groundCheckDistance;
    [SerializeField] private LayerMask _layerToDetect;
    [SerializeField] private float _wallCheckDistance;
    [SerializeField] private Transform _primaryWallCheck;
    [SerializeField] private Transform _secondaryWallCheck;
    public bool GroundDetected { get; private set; }
    public bool WallDetected { get; private set; }


    #region Components

    [field: Header("Components")]
    [field: SerializeField] public Animator Anim { get; private set; }
    [field: SerializeField] public Rigidbody2D RB { get; private set; }

    #endregion

    private bool _isFacingRight = true;
    public int FacingDir { get; private set; } = 1;



    protected virtual void Awake() {

        if (Anim == null) Anim = GetComponentInChildren<Animator>();
        if (RB == null) RB = GetComponent<Rigidbody2D>();

        StateMachine = new StateMachine();
    }

    protected virtual void Start() {

    }


    protected virtual void Update() {
        HandleCollisionDetection();
        StateMachine.UpdateActiveState();
    }

    public void SetVelocity(float xVelocity, float yVelocity) {
        RB.linearVelocity = new Vector2(xVelocity, yVelocity);
        HandleFlip(xVelocity);
    }

    public void CallAnimationTrigger() {
        StateMachine.CurrentState.CallAnimationTrigger();
    }

    private void HandleFlip(float xVelocity) {

        // Flip character if horizontal velocity is towards the right but player is facing left or  if horizontal velocity is towards the left but the player is facing right
        if ((xVelocity > 0 && !_isFacingRight) || (xVelocity < 0 && _isFacingRight))
            FlipCharacter();
    }

    public void FlipCharacter() {
        transform.Rotate(0, 180, 0);
        _isFacingRight = !_isFacingRight;
        FacingDir = -FacingDir;
    }

    private void HandleCollisionDetection() {
        GroundDetected = Physics2D.Raycast(_groundCheckTransform.position, Vector3.down, _groundCheckDistance, _layerToDetect);

        if (_secondaryWallCheck != null)
            WallDetected = Physics2D.Raycast(_primaryWallCheck.position, Vector3.right * FacingDir, _wallCheckDistance, _layerToDetect)
                        && Physics2D.Raycast(_secondaryWallCheck.position, Vector3.right * FacingDir, _wallCheckDistance, _layerToDetect);
        else
            WallDetected = Physics2D.Raycast(_primaryWallCheck.position, Vector3.right * FacingDir, _wallCheckDistance, _layerToDetect);
    }


    private void OnDrawGizmos() {
        Gizmos.DrawLine(_groundCheckTransform.position, _groundCheckTransform.position + new Vector3(0, -_groundCheckDistance));
        Gizmos.DrawLine(_primaryWallCheck.position, _primaryWallCheck.position + new Vector3(FacingDir * _wallCheckDistance, 0));

        if(_secondaryWallCheck != null)
            Gizmos.DrawLine(_secondaryWallCheck.position, _secondaryWallCheck.position + new Vector3(FacingDir * _wallCheckDistance, 0));
    }

}
