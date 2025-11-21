using UnityEngine;

public class SkillObjectBase : MonoBehaviour
{
    [SerializeField] private GameObject _onHitVFX;
    [Space]
    [SerializeField] protected LayerMask enemyLayer;
    [SerializeField] protected Transform targetCheck;
    [SerializeField] protected float checkRadius = 1f;

    protected Animator anim;
    protected EntityStats playerStats;
    protected EntityVFX entityVFX;
    protected DamageScaleData damageScaleData;
    protected E_ElementType usedElement;
    protected bool targetTookDamage;

    protected virtual void Awake() {
        anim = GetComponentInChildren<Animator>();
    }

    protected Collider2D[] GetEnemiesAround(Transform t, float radius) {
        return Physics2D.OverlapCircleAll(t.position, radius, enemyLayer);
    }

    protected void DamageEnemiesInRadius(Transform t, float radius) {
        foreach(var target in GetEnemiesAround(t, radius)) {
            if(target.TryGetComponent<IDamagable>(out var damagable)) {
                ElementalEffectData elementalEffectData = new ElementalEffectData(playerStats, damageScaleData);

                float physicalDamage = playerStats.GetPhysicalDamage(out bool isCritDamage, damageScaleData.physical);
                float elementalDamage = playerStats.GetElementalDamage(out E_ElementType elementType, damageScaleData.elemental);

                usedElement = elementType;

                targetTookDamage = damagable.TakeDamage(physicalDamage, elementalDamage, elementType, transform);

                if (elementType != E_ElementType.None)
                    target.GetComponent<EntityStatusHandler>()?.ApplyStatusEffect(elementType, elementalEffectData);

                if (targetTookDamage)
                    entityVFX.CreateOnHitVFX(target.transform, isCritDamage, elementType);

                
            }
        }
    }

    protected Transform FindClosestTarget() {
        Transform target = null;
        float closestDistance = Mathf.Infinity;

        // Checking for all enemies within a 10 meter radius of the player
        foreach(var enemy in  GetEnemiesAround(transform, 10f)) {
            float distance = Vector2.Distance(transform.position, enemy.transform.position);
            
            if(distance < closestDistance) {
                closestDistance = distance;
                target = enemy.transform;
            }
        }

        return target;

    }

    protected virtual void OnDrawGizmos() {
        if(targetCheck == null)
            targetCheck = transform;

        Gizmos.DrawWireSphere(targetCheck.position, checkRadius);
    }
}
