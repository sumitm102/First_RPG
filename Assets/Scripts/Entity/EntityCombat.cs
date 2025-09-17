using UnityEngine;

public class EntityCombat : MonoBehaviour
{
    [field: Header("Target detection")]
    [field: SerializeField] public Transform TargetCheckTransform { get; private set; }
    [field: SerializeField] public float TargetCheckRadius { get; private set; } = 1f;
    [field: SerializeField] public LayerMask TargetDetectionLayer { get; private set; }


    [field: Header("Status effect details")]
    [field: SerializeField] public float DefaultDuration { get; private set; } = 3f;
    [field: SerializeField] public float ChillSlowMultiplier { get; private set; } = 0.2f;


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

                if (elementType != ElementType.None)
                    ApplyStatusEffect(target.transform, elementType);

                if (targetTookDamage) {
                    _vfx.UpdateOnHitColor(elementType);
                    _vfx.CreateOnHitVFX(target.transform, isCritDamage);
                }
            }
        }
    }

    public void ApplyStatusEffect(Transform target, ElementType elementType) {
        EntityStatusHandler entityStatusHandler = target.GetComponent<EntityStatusHandler>();
        if (entityStatusHandler == null)
            return;

        if (elementType == ElementType.Ice && entityStatusHandler.CanStatusEffectBeApplied(elementType))
            entityStatusHandler.ApplyChilledEffect(DefaultDuration, ChillSlowMultiplier);
    }
    
    protected Collider2D[] GetDetectedColliders() {
        return Physics2D.OverlapCircleAll(TargetCheckTransform.position, TargetCheckRadius, TargetDetectionLayer);
    }

    private void OnDrawGizmos() {
        Gizmos.DrawWireSphere(TargetCheckTransform.position, TargetCheckRadius);
    }


}
