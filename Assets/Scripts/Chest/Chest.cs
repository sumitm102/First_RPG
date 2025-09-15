using UnityEngine;

public class Chest : MonoBehaviour, IDamagable
{
    private readonly static int _chestOpenHash = Animator.StringToHash("ChestOpen");

    private Animator _anim;
    private Rigidbody2D _rb;
    private EntityVFX _entityVFX;

    [field: Header("Open Details")]
    [field: SerializeField] public Vector2 KnockbackVelocity { get; private set; }

    private void Awake() {
        _anim = GetComponentInChildren<Animator>();
        _rb = GetComponent<Rigidbody2D>();
        _entityVFX = GetComponent<EntityVFX>();
    }

    public bool TakeDamage(float damage, float elementalDamage, ElementType element, Transform damageDealer) {

        if (_anim != null)
            _anim.SetBool(_chestOpenHash, true);

        if (_rb != null) {
            _rb.linearVelocity = KnockbackVelocity;
            _rb.angularVelocity = Random.Range(-100f, 100f);
        }

        if (_entityVFX != null)
            _entityVFX.PlayOnDamageVFX();

        // Drop items

        return true;
    }

}
