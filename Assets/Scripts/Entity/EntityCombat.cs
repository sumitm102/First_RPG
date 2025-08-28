using UnityEngine;

public class EntityCombat : MonoBehaviour
{
    [field: Header("Target detection")]
    [field: SerializeField] public Transform TargetCheckTransform { get; private set; }
    [field: SerializeField] public float TargetCheckRadius { get; private set; } = 1f;
    [field: SerializeField] public LayerMask TargetDetectionLayer { get; private set; }
    [field: SerializeField] public int Damage { get; private set; } = 10;

    private Collider2D[] _targetColliders;

    // May not be necessary
    //private EntityHealth _targetHealth;
    private EntityVFX _vfx;

    private void Awake() {
        _vfx = GetComponent<EntityVFX>();
    }




    public void PerformAttack() {
        _targetColliders = GetDetectedColliders();

        foreach(var target in _targetColliders) {

            if (target.TryGetComponent<IDamagable>(out var damagable)) {
                damagable.TakeDamage(Damage, transform);

                _vfx.CreateOnHitVFX(target.transform);
            }

            //if (target.TryGetComponent<EntityHealth>(out var targetHealth))
            //    targetHealth.TakeDamage(Damage, transform);
        }
    }
    
    protected Collider2D[] GetDetectedColliders() {
        return Physics2D.OverlapCircleAll(TargetCheckTransform.position, TargetCheckRadius, TargetDetectionLayer);
    }

    private void OnDrawGizmos() {
        Gizmos.DrawWireSphere(TargetCheckTransform.position, TargetCheckRadius);
    }


}
