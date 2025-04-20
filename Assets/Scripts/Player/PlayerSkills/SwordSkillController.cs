using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class SwordSkillController : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private Rigidbody2D _rbody;
    [SerializeField] private CircleCollider2D _circleCollider;
    private Player _player;

    private bool _canRotate = true;
    private bool _isReturning;

    private float _returnSpeed = 12f;
    private float _freezeTimeDuration;

    [Header("Pierce info")]
    private int _pierceAmount;

    [Header("Bounce info")]
    private float _bounceSpeed;
    private bool _isBouncing;
    private int _bounceAmount;
    private List<Transform> _enemyTargetList;
    private int _targetIndex;

    [Header("Spin info")]
    private float _maxTravelDistance;
    private float _spinDuration;
    private float _spinTimer;
    private bool _wasStopped;
    private bool _isSpinning;

    private float _damageTimer;
    private float _damageCooldown;

    private float _spinDirection;

    private void Awake() {
        _animator = GetComponentInChildren<Animator>();
        _rbody = GetComponent<Rigidbody2D>();
        _circleCollider = GetComponent<CircleCollider2D>();
    }

    private void DestroySword() {
        Destroy(this.gameObject);
    }

    private void Update() {

        //For rotating the sword along the aiming path
        if (_canRotate)
            transform.right = _rbody.linearVelocity;

        if (_isReturning) {
            transform.position = Vector2.MoveTowards(transform.position, _player.transform.position, _returnSpeed * Time.deltaTime);

            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 1f);

            foreach (var collider in colliders) {
                if (collider.TryGetComponent<Enemy>(out Enemy _surroundingEnemy))
                    _surroundingEnemy.DamageEffect();

            }

            if ((_player.transform.position - transform.position).sqrMagnitude < 0.5f * 0.5f) {
                _player.CatchSword();
            }

        }

        BounceLogic();
        SpinLogic();
    }

    private void SpinLogic() {
        if (_isSpinning) {
            if ((_player.transform.position - transform.position).sqrMagnitude > _maxTravelDistance * _maxTravelDistance && !_wasStopped) {
                StopWhenSpinning();
            }

            if (_wasStopped) {
                _spinTimer -= Time.deltaTime;

                //To move the sword little by little when spinning after surpassing max distance
                transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x + _spinDirection, transform.position.y), 1f * Time.deltaTime);

                if (_spinTimer < 0) {
                    _isReturning = true;
                    _isSpinning = false;
                    //_wasStopped = false;
                }

                _damageTimer -= Time.deltaTime;

                if (_damageTimer < 0) {
                    _damageTimer = _damageCooldown;


                    //To check for nearby enemies while the sword is spinning
                    Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 1f);

                    foreach (var collider in colliders) {
                        if (collider.TryGetComponent<Enemy>(out Enemy _surroundingEnemy)) {
                            DamageAndFreezeEnemy(_surroundingEnemy);

                        }


                    }

                }
            }
        }
    }

    private void StopWhenSpinning() {
        _wasStopped = true;
        _rbody.constraints = RigidbodyConstraints2D.FreezePosition;
        _spinTimer = _spinDuration;
    }

    private void BounceLogic() {
        if (_isBouncing && _enemyTargetList.Count > 0) {
            transform.position = Vector2.MoveTowards(transform.position, _enemyTargetList[_targetIndex].position, _bounceSpeed * Time.deltaTime);

            if ((_enemyTargetList[_targetIndex].position - transform.position).sqrMagnitude < 0.1f * 0.1f) {
                
                //Damaging the enemy and freezing it before switching to the next target
                DamageAndFreezeEnemy(_enemyTargetList[_targetIndex].GetComponent<Enemy>());

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


        if(_collider.TryGetComponent<Enemy>(out Enemy enemy)) {
            DamageAndFreezeEnemy(enemy);
        }


        if (_collider.TryGetComponent<Enemy>(out Enemy _enemy) && _isBouncing && _enemyTargetList.Count <= 0) {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 10f);

            foreach (var collider in colliders) {
                if (collider.TryGetComponent<Enemy>(out Enemy _surroundingEnemy))
                    _enemyTargetList.Add(_surroundingEnemy.transform);

            }

        }

        StuckInto(_collider);
    }

    private void DamageAndFreezeEnemy(Enemy enemy) {
        enemy.DamageEffect();

        //Ignoring the return value of the coroutine
        _ = enemy.StartCoroutine("FreezeTimeFor", _freezeTimeDuration);
    }

    private void StuckInto(Collider2D _collider) {

        if (_pierceAmount > 0 && _collider.TryGetComponent<Enemy>(out Enemy enmey)) {
            _pierceAmount--;
            return;
        }

        if (_isSpinning) {
            StopWhenSpinning();
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


    #region Setup Swords
    public void SetupSword(Vector2 _dir, float _gravityScale, Player _player, float _freezeTimeDuration, float _returnSpeed) {
        this._player = _player;
        this._freezeTimeDuration = _freezeTimeDuration;
        this._returnSpeed = _returnSpeed;

        _rbody.linearVelocity = _dir;
        _rbody.gravityScale = _gravityScale;

        if(_pierceAmount <= 0)
            _animator.SetBool("Rotation", true);

        _spinDirection = Mathf.Clamp(_rbody.linearVelocityX, -1f, 1f);

        //If sword doesn't return after throwing for 10 seconds, destroy it
        Invoke("DestroySword", 10f);
    }

    public void SetupBounce(bool _isBouncing, int _amountOfBounces, float _bounceSpeed) {
        this._isBouncing = _isBouncing;
        this._bounceSpeed = _bounceSpeed;
        _bounceAmount = _amountOfBounces;

        //Needs a default value when it's private
        _enemyTargetList = new List<Transform>();
    }

    public void SetupPierce(int _amountOfPierce) {
        _pierceAmount = _amountOfPierce;
    }

    public void SetupSpin(bool _isSpinning, float _maxTravelDistance, float _spinDuration, float _damageCooldown) {
        this._isSpinning = _isSpinning;
        this._maxTravelDistance = _maxTravelDistance;
        this._spinDuration = _spinDuration;
        this._damageCooldown = _damageCooldown;
    }

    #endregion

    public void ReturnSword() {

        _rbody.constraints = RigidbodyConstraints2D.FreezeAll;
        _animator.SetBool("Rotation", true);
        transform.parent = null;
        _isReturning = true;
    }
}
