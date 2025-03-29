using UnityEngine;

public class SwordSkillController : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private Rigidbody2D _rbody;
    [SerializeField] private CircleCollider2D _circleCollider;
    private Player player;

    private void Awake() {
        _animator = GetComponentInChildren<Animator>();
        _rbody = GetComponent<Rigidbody2D>();
        _circleCollider = GetComponent<CircleCollider2D>();
    }

    public void SetupSword(Vector2 _dir, float _gravityScale) {
        _rbody.linearVelocity = _dir;
        _rbody.gravityScale = _gravityScale;
    }
}
