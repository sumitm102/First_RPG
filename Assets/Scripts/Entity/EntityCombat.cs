using UnityEngine;

public class EntityCombat : MonoBehaviour
{
    [field: Header("Target detection")]
    [field: SerializeField] public Transform TargetCheckTransform { get; private set; }
    [field: SerializeField] public float TargetCheckRadius { get; private set; } = 1f;
    [field: SerializeField] public LayerMask TargetDetectionLayer { get; private set; }
    [field: SerializeField] public int Damage { get; private set; } = 10;

    private Collider2D[] _targetColliders;
    private EntityHealth _targetHealth;


    public void PerformAttack() {
        _targetColliders = GetDetectedColliders();

        foreach(var target in _targetColliders) {
            _targetHealth = target.GetComponent<EntityHealth>();

            if (_targetHealth != null)
                _targetHealth.TakeDamage(Damage, transform);
        }
    }
    
    private Collider2D[] GetDetectedColliders() {
        return Physics2D.OverlapCircleAll(TargetCheckTransform.position, TargetCheckRadius, TargetDetectionLayer);
    }

    private void OnDrawGizmos() {
        Gizmos.DrawWireSphere(TargetCheckTransform.position, TargetCheckRadius);
    }


}
