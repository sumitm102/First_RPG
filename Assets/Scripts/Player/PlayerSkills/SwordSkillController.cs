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

    [Header("Pierce info")]
    [SerializeField] private int _pierceAmount;

    [Header("Bounce info")]
    [SerializeField] private float _bounceSpeed;
    private bool _isBouncing;
    private int _bounceAmount;
    private List<Transform> _enemyTargetList;
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
        if (_isBouncing && _enemyTargetList.Count > 0) {
            transform.position = Vector2.MoveTowards(transform.position, _enemyTargetList[_targetIndex].position, _bounceSpeed * Time.deltaTime);

            if ((_enemyTargetList[_targetIndex].position - transform.position).sqrMagnitude < 0.1f * 0.1f) {
                _targetIndex++;
                _bounceAmount--;

                if (_bounceAmount <= 0) {
                    _isBouncing = false;
                    _isReturning = true; //For returning to the player when all bounces have been performed
                }

                if (_targetIndex >= _enemyTargetList.Count)
                    _targetIndex = 0;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D _collider) {

        if (_isReturning) return;

        _collider.GetComponent<Enemy>()?.Damage();

        if (_collider.TryGetComponent<Enemy>(out Enemy _enemy)) {

            if (_isBouncing && _enemyTargetList.Count <= 0) {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 10f);

                foreach (var collider in colliders) {
                    if (collider.TryGetComponent<Enemy>(out Enemy _surroundingEnemy))
                        _enemyTargetList.Add(_surroundingEnemy.transform);
                    
                }
            }
        }

        StuckInto(_collider);
    }

    private void StuckInto(Collider2D _collider) {

        if (_pierceAmount > 0 && _collider.TryGetComponent<Enemy>(out Enemy enmey)) {
            _pierceAmount--;
            return;
        }


        _canRotate = false;
        _circleCollider.enabled = false;

        //Changing rigidbody type to kinematic
        _rbody.bodyType = RigidbodyType2D.Kinematic;
        _rbody.constraints = RigidbodyConstraints2D.FreezeAll;

        if (_isBouncing && _enemyTargetList.Count > 0) return;

        _animator.SetBool("Rotation", false);

        //Setting the parent to the object collided with
        transform.parent = _collider.transform;
    }

    public void SetupSword(Vector2 _dir, float _gravityScale, Player _player) {
        this._player = _player;

        _rbody.linearVelocity = _dir;
        _rbody.gravityScale = _gravityScale;

        if(_pierceAmount <= 0)
            _animator.SetBool("Rotation", true);
    }

    public void SetupBounce(bool _isBouncing, int _amountOfBounces) {
        this._isBouncing = _isBouncing;
        _bounceAmount = _amountOfBounces;

        //Needs a default value when it's private
        _enemyTargetList = new List<Transform>();
    }

    public void SetupPierce(int _amountOfPierce) {
        _pierceAmount = _amountOfPierce;
    }

    public void ReturnSword() {

        _rbody.constraints = RigidbodyConstraints2D.FreezeAll;
        _animator.SetBool("Rotation", true);
        transform.parent = null;
        _isReturning = true;
    }
}
