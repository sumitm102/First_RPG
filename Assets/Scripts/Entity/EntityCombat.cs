using UnityEngine;

public class EntityCombat : MonoBehaviour
{
    [field: Header("Target detection")]
    [field: SerializeField] public Transform TargetCheckTransform { get; private set; }
    [field: SerializeField] public float TargetCheckRadius { get; private set; } = 1f;
    [field: SerializeField] public LayerMask TargetDetectionLayer { get; private set; }

    private Collider2D[] _targetColliders;

    // May not be necessary
    //private EntityHealth _targetHealth;

    private EntityVFX _vfx;
    private EntityStats _entityStats;

    private void Awake() {
        _vfx = GetComponent<EntityVFX>();
        _entityStats = GetComponent<EntityStats>();
    }




    public void PerformAttack() {
        _targetColliders = GetDetectedColliders();

        foreach(var target in _targetColliders) {

            if (target.TryGetComponent<IDamagable>(out var damagable)) {

                float physicalDamage = _entityStats.GetPhysicalDamage(out bool isCritDamage);
                float elementalDamage = _entityStats.GetElementalDamage(out ElementType elementType);

                bool targetTookDamage = damagable.TakeDamage(physicalDamage, elementalDamage, elementType, transform);

                if (targetTookDamage)
                    _vfx.CreateOnHitVFX(target.transform, isCritDamage);
            }
        }
    }
    
    protected Collider2D[] GetDetectedColliders() {
        return Physics2D.OverlapCircleAll(TargetCheckTransform.position, TargetCheckRadius, TargetDetectionLayer);
    }

    private void OnDrawGizmos() {
        Gizmos.DrawWireSphere(TargetCheckTransform.position, TargetCheckRadius);
    }


}
