using UnityEngine;

public class EntityCombat : MonoBehaviour
{
    [field: Header("Target detection")]
    [field: SerializeField] public Transform TargetCheckTransform { get; private set; }
    [field: SerializeField] public float TargetCheckRadius { get; private set; } = 1f;
    [field: SerializeField] public LayerMask TargetDetectionLayer { get; private set; }

    private Collider2D[] _targetColliders;


    public void PerformAttack() {
        _targetColliders = GetDetectedColliders();

        foreach(var target in _targetColliders) {
            Debug.Log("Attacking " + target.name);
        }
    }
    
    private Collider2D[] GetDetectedColliders() {
        return Physics2D.OverlapCircleAll(TargetCheckTransform.position, TargetCheckRadius, TargetDetectionLayer);
    }

    private void OnDrawGizmos() {
        Gizmos.DrawWireSphere(TargetCheckTransform.position, TargetCheckRadius);
    }


}
