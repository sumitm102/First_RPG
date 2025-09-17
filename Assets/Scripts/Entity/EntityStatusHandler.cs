using System.Collections;
using UnityEngine;

public class EntityStatusHandler : MonoBehaviour
{
    private Entity _entity;
    private EntityVFX _entityVFX;
    private EntityStats _entityStats;
    private ElementType _currentEffect = ElementType.None;

    private void Awake() {
        _entity = GetComponent<Entity>();
        _entityVFX = GetComponent<EntityVFX>();
        _entityStats = GetComponent<EntityStats>();
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

    public bool CanStatusEffectBeApplied(ElementType elementType) {
        return _currentEffect == ElementType.None;
    }
}
