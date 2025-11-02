using UnityEngine;

public class SkillObjectBase : MonoBehaviour
{
    [SerializeField] protected LayerMask enemyLayer;
    [SerializeField] protected Transform targetCheck;
    [SerializeField] protected float checkRadius = 1f;

    protected Collider2D[] EnemiesAround(Transform t, float radius) {
        return Physics2D.OverlapCircleAll(t.position, radius, enemyLayer);
    }

    protected void DamageEnemiesInRadius(Transform t, float radius) {
        foreach(var target in EnemiesAround(t, radius)) {
            if(target.TryGetComponent<IDamagable>(out var damagable)) {
                damagable.TakeDamage(1, 1, E_ElementType.None, transform);
            }
        }
    }

    protected virtual void OnDrawGizmos() {
        if(targetCheck == null)
            targetCheck = transform;

        Gizmos.DrawWireSphere(targetCheck.position, checkRadius);
    }
}
