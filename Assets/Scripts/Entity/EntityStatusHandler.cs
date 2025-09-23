using System.Collections;
using UnityEngine;

public class EntityStatusHandler : MonoBehaviour
{
    private Entity _entity;
    private EntityVFX _entityVFX;
    private EntityStats _entityStats;
    private EntityHealth _entityHealth;
    private ElementType _currentEffect = ElementType.None;

    private void Awake() {
        _entity = GetComponent<Entity>();
        _entityVFX = GetComponent<EntityVFX>();
        _entityStats = GetComponent<EntityStats>();
        _entityHealth = GetComponent<EntityHealth>();
    }


    public void ApplyChilledEffect(float duration, float slowMultiplier) {
        float iceResistance = _entityStats.GetElementalResistance(ElementType.Ice);
        float reducedDuration = duration * (1f - iceResistance);

        StartCoroutine(ChilledEffectCo(reducedDuration, slowMultiplier));
    }

    private IEnumerator ChilledEffectCo(float duration, float slowMultiplier) {

        _entity.SlowDownEntity(duration, slowMultiplier);
        _currentEffect = ElementType.Ice;
        _entityVFX.PlayOnStatusVFX(duration, _currentEffect);

        yield return new WaitForSeconds(duration);

        _currentEffect = ElementType.None;
    }

    public void ApplyBurnEffect(float duration, float totalDamage) {
        float fireResistance = _entityStats.GetElementalResistance(ElementType.Fire);
        float reducedDamage = totalDamage * (1f - fireResistance);

        StartCoroutine(BurnEffectCo(duration, reducedDamage));
    }

    private IEnumerator BurnEffectCo(float duration, float totalDamage) {
        _currentEffect = ElementType.Fire;
        _entityVFX.PlayOnStatusVFX(duration, _currentEffect);

        int ticksPerSecond = 2;
        int tickCount = Mathf.RoundToInt(ticksPerSecond * duration);

        float damagePerTick = totalDamage / tickCount;
        float tickInterval = 1f / ticksPerSecond;

        for (int i = 0; i < tickCount; i++) {
            // Reduce health of entity
            _entityHealth.ReduceHealth(damagePerTick);
            yield return new WaitForSeconds(tickInterval);
        }

        _currentEffect = ElementType.None;
    }

    public bool CanStatusEffectBeApplied(ElementType elementType) {
        return _currentEffect == ElementType.None;
    }
}
