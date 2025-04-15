using Unity.VisualScripting;
using UnityEngine;

public class CrystalSkillController : MonoBehaviour
{
    private float _crystalExistTimer;
    private float _moveSpeed;
    private bool _canMoveToEnemy;
    private bool _canExplode;

    private bool _canGrow;
    private float _growSpeed;

    private Transform _closestEnemy;

    private Animator _anim => GetComponent<Animator>();
    private CircleCollider2D _circleCollider => GetComponent<CircleCollider2D>();
    public void SetupCrystal(float _crystalDuration, bool _canExplode, bool _canMoveToEnemy, float _moveSpeed, float _growSpeed, Transform _closestEnemy) {
        _crystalExistTimer = _crystalDuration;
        this._canExplode = _canExplode;
        this._canMoveToEnemy = _canMoveToEnemy;
        this._moveSpeed = _moveSpeed;
        this._growSpeed = _growSpeed;
        this._closestEnemy = _closestEnemy;
    }

    private void Update() {
        _crystalExistTimer -= Time.deltaTime;
        
        if (_crystalExistTimer < 0) 
            ExplodeOrSelfDestroy();

        if (_canMoveToEnemy && _closestEnemy != null) {
            transform.position = Vector2.Lerp(transform.position, _closestEnemy.position, _moveSpeed * Time.deltaTime);

            if ((_closestEnemy.position - transform.position).sqrMagnitude < 1.2f * 1.2f) {
                _canMoveToEnemy = false;
                ExplodeOrSelfDestroy();
            }

        }

        if (_canGrow) 
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(3f, 3f), _growSpeed * Time.deltaTime); 
        

    }



    //Used in an event on crystal explode animation
    private void AnimationExplodeEvent() {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, _circleCollider.radius);

        foreach (var collider in colliders) {
            if (collider.TryGetComponent<Enemy>(out Enemy enemy))
                enemy.Damage();
        }
    }

    public void ExplodeOrSelfDestroy() {

        //Only grows when crystal is able to explode
        if (_canExplode) {
            _canGrow = true;
            _anim.SetTrigger("Explode");
        }
        else
            SelfDestroy();
    }

    public void SelfDestroy() {
        Destroy(this.gameObject);
    }
}
