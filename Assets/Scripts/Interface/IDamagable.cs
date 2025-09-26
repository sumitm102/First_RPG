using UnityEngine;

public interface IDamagable
{
    public bool TakeDamage(float damage, float elementalDamage, E_ElementType elementType, Transform damageDealer);
}
