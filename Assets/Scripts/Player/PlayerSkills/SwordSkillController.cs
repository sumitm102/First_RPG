using NUnit.Framework;
using System.Collections.Generic;
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

    [Header("Bounce info")]
    [SerializeField] private float _bounceSpeed;
    private bool isBouncing;
    private int amountOfBounces;
    private List<Transform> enemyTargetList;
    private int _targetIndex;

    private void Awake() {
        _animator = GetComponentInChildren<Animator>();
        _rbody = GetComponent<Rigidbody2D>();
        _circleCollider = GetComponent<CircleCollider2D>();
    }

    private void Update() {

        //For rotating the sword along the aiming path
        if (_canRotate)
            transform.right = _rbody.linearVelocity;

        if (_isReturning) {
            transform.position = Vector2.MoveTowards(transform.position, _player.transform.position, _returnSpeed * Time.deltaTime);

            if ((_player.transform.position - transform.position).sqrMagnitude < 0.5f * 0.5f) {
                _player.CatchSword();
            }

        }

        BounceLogic();
    }

    private void BounceLogic() {
        if (isBouncing && enemyTargetList.Count > 0) {
            transform.position = Vector2.MoveTowards(transform.position, enemyTargetList[_targetIndex].position, _bounceSpeed * Time.deltaTime);

            if ((enemyTargetList[_targetIndex].position - transform.position).sqrMagnitude < 0.1f * 0.1f) {
                _targetIndex++;
                amountOfBounces--;

                if (amountOfBounces <= 0) {
                    isBouncing = false;
                    _isReturning = true; //For returning to the player when all bounces have been performed
                }

                if (_targetIndex >= enemyTargetList.Count)
                    _targetIndex = 0;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D _collider) {

        if (_isReturning) return;


        if (_collider.TryGetComponent<Enemy>(out Enemy _enemy)) {

            if (isBouncing && enemyTargetList.Count <= 0) {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 10f);

                foreach (var collider in colliders) {
                    if (collider.TryGetComponent<Enemy>(out Enemy _surroundingEnemy))
                        enemyTargetList.Add(_surroundingEnemy.transform);
                    
                }
            }
        }

        StuckInto(_collider);
    }

    private void StuckInto(Collider2D _collider) {

        _canRotate = false;
        _circleCollider.enabled = false;

        //Changing rigidbody type to kinematic
        _rbody.bodyType = RigidbodyType2D.Kinematic;
        _rbody.constraints = RigidbodyConstraints2D.FreezeAll;

        if (isBouncing && enemyTargetList.Count > 0) return;

        _animator.SetBool("Rotation", false);

        //Setting the parent to the object collided with
        transform.parent = _collider.transform;
    }

    public void SetupSword(Vector2 _dir, float _gravityScale, Player _player) {
        this._player = _player;

        _rbody.linearVelocity = _dir;
        _rbody.gravityScale = _gravityScale;

        _animator.SetBool("Rotation", true);
    }

    public void SetupBounce(bool _isBouncing, int _amountOfBounces) {
        isBouncing = _isBouncing;
        amountOfBounces = _amountOfBounces;

        //Needs a default value when it's private
        enemyTargetList = new List<Transform>();
    }

    public void ReturnSword() {

        _rbody.constraints = RigidbodyConstraints2D.FreezeAll;
        _animator.SetBool("Rotation", true);
        transform.parent = null;
        _isReturning = true;
    }
}
