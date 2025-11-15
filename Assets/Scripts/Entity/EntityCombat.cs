using UnityEngine;

public class EntityCombat : MonoBehaviour
{
    [field: Header("Target detection")]
    [field: SerializeField] public Transform TargetCheckTransform { get; private set; }
    [field: SerializeField] public float TargetCheckRadius { get; private set; } = 1f;
    [field: SerializeField] public LayerMask TargetDetectionLayer { get; private set; }


    //[field: Header("Status effect details")]
    //[field: SerializeField] public float DefaultDuration { get; private set; } = 3f;
    //[field: SerializeField] public float ChillSlowMultiplier { get; private set; } = 0.2f;
    //[field: SerializeField] public float ElectrifyChargeAmount { get; private set; } = 0.4f;

    //[field: Space]
    //[field: SerializeField] public float FireScaleFactor { get; private set; } = 0.8f;
    //[field: SerializeField] public float LightningScaleFactor { get; private set; } = 2f;



    private Collider2D[] _targetColliders;

    // May not be necessary
    //private EntityHealth _targetHealth;

    private EntityVFX _entityVFX;
    private EntityStats _entityStats;

    public DamageScaleData basicAttackScale;

    private void Awake() {
        _entityVFX = GetComponent<EntityVFX>();
        _entityStats = GetComponent<EntityStats>();
    }




    public void PerformAttack() {
        _targetColliders = GetDetectedColliders();

        foreach(var target in _targetColliders) {

            if (target.TryGetComponent<IDamagable>(out var damagable)) {

                ElementalEffectData elementalEffectData = new ElementalEffectData(_entityStats, basicAttackScale);

                float physicalDamage = _entityStats.GetPhysicalDamage(out bool isCritDamage);
                float elementalDamage = _entityStats.GetElementalDamage(out E_ElementType elementType, 0.6f); // Perform 60% of the original elemental damage
                bool targetTookDamage = damagable.TakeDamage(physicalDamage, elementalDamage, elementType, transform);

                if (elementType != E_ElementType.None) 
                    target.GetComponent<EntityStatusHandler>()?.ApplyStatusEffect(elementType, elementalEffectData);
                

                if (targetTookDamage) 
                    _entityVFX.CreateOnHitVFX(target.transform, isCritDamage, elementType);
                
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
