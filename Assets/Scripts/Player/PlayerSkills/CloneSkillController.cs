using UnityEngine;

public class CloneSkillController : MonoBehaviour {

    [SerializeField] private float _transparencySpeed;
    private float _cloneTimer;

    [SerializeField] private Transform _attackCheck;
    [SerializeField] private float _attackCheckRadius = 0.85f;
    private Transform _closestEnemy;

    #region Components
    private SpriteRenderer _spriteRenderer;
    private Animator _animator;

    #endregion

    private void Awake() {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
    }

    private void Update() {
        _cloneTimer -= Time.deltaTime;

        if(_cloneTimer < 0) {

            //Clone gets more transparent over time
            _spriteRenderer.color = new Color(1, 1, 1, _spriteRenderer.color.a - (Time.deltaTime * _transparencySpeed));

            //Destroy clone after becoming fully transparent
            if (_spriteRenderer.color.a <= 0)
                Destroy(this.gameObject);
        }
    }


    public void SetupClone(Transform _newTransform, float  _cloneDuration, bool _canAttack, Vector3 _offset, Transform _closestEnemy) {

        //Randomly performs one of the three attacks when instantiated assuming player is able to attack
         if(_canAttack) _animator.SetInteger("AttackNumber", Random.Range(1, 4));

        transform.position = _newTransform.position + _offset;
        _cloneTimer = _cloneDuration;
        this._closestEnemy  = _closestEnemy;

        FaceClosestTarget();
    }

    private void AnimationTrigger() {
        _cloneTimer = -0.1f;
    }

    private void AttackTrigger() {

        Collider2D[] colliders = Physics2D.OverlapCircleAll(_attackCheck.position, _attackCheckRadius);

        foreach (var collider in colliders) {
            if (collider.TryGetComponent<Enemy>(out Enemy enemy))
                enemy.Damage();
        }

    }

    private void FaceClosestTarget() {

        if (_closestEnemy != null && transform.position.x > _closestEnemy.position.x)
            transform.Rotate(0, 180, 0);
        
    }


    private void OnDrawGizmos() {
        Gizmos.DrawWireSphere(_attackCheck.position, _attackCheckRadius);
    }

}
