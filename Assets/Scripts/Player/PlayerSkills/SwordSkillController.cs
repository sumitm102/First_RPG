using UnityEngine;

public class SwordSkillController : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private Rigidbody2D _rbody;
    [SerializeField] private CircleCollider2D _circleCollider;
    [SerializeField] private float _returnSpeed = 12f;
    private Player _player;

    private bool _canRotate = true;
    private bool _isReturning;

    private void Awake() {
        _animator = GetComponentInChildren<Animator>();
        _rbody = GetComponent<Rigidbody2D>();
        _circleCollider = GetComponent<CircleCollider2D>();
    }

    private void Update() {

        //For rotating the sword along the aiming path

        if(_canRotate) 
            transform.right = _rbody.linearVelocity;

        if (_isReturning) {
            transform.position = Vector2.MoveTowards(transform.position,  _player.transform.position, _returnSpeed * Time.deltaTime);

            if ((_player.transform.position - transform.position).sqrMagnitude < 0.5f * 0.5f) {
                _player.CatchSword();
            }

        }
    }

    private void OnTriggerEnter2D(Collider2D _collider) {

        if (_isReturning) return;

        _animator.SetBool("Rotation", false);

        _canRotate = false;
        _circleCollider.enabled = false;

        //Changing rigidbody type to kinematic
        _rbody.bodyType = RigidbodyType2D.Kinematic;
        _rbody.constraints = RigidbodyConstraints2D.FreezeAll;

        //Setting the parent to the object collided with
        transform.parent = _collider.transform;
    }

    public void SetupSword(Vector2 _dir, float _gravityScale, Player _player) {
        this._player = _player;

        _rbody.linearVelocity = _dir;
        _rbody.gravityScale = _gravityScale;

        _animator.SetBool("Rotation", true);
    }

    public void ReturnSword() {

        _rbody.constraints = RigidbodyConstraints2D.FreezeAll;
        transform.parent = null;
        _isReturning = true;
    }
}
