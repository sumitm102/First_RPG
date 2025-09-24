using System.Collections;
using UnityEngine;

public class EntityStatusHandler : MonoBehaviour
{
    private Entity _entity;
    private EntityVFX _entityVFX;
    private EntityStats _entityStats;
    private EntityHealth _entityHealth;
    private ElementType _currentEffect = ElementType.None;

    [Header("Electrify effect details")]
    [SerializeField] private GameObject _lightningStrikeVFX;
    [SerializeField] private float _currentCharge;
    [SerializeField] private float _maximumCharge = 1f;
    private Coroutine _electrifyCoroutine;

    private void Awake() {
        _entity = GetComponent<Entity>();
        _entityVFX = GetComponent<EntityVFX>();
        _entityStats = GetComponent<EntityStats>();
        _entityHealth = GetComponent<EntityHealth>();
    }

    #region Applying Chill Effect

    public void ApplyChillEffect(float duration, float slowMultiplier) {
        float iceResistance = _entityStats.GetElementalResistance(ElementType.Ice);
        float reducedDuration = duration * (1f - iceResistance);

        StartCoroutine(ChillEffectCo(reducedDuration, slowMultiplier));
    }

    private IEnumerator ChillEffectCo(float duration, float slowMultiplier) {

        _entity.SlowDownEntity(duration, slowMultiplier);
        _currentEffect = ElementType.Ice;
        _entityVFX.PlayOnStatusVFX(duration, _currentEffect);

        yield return new WaitForSeconds(duration);

        _currentEffect = ElementType.None;
    }

    #endregion

    #region Applying Burn Effect

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

    #endregion


    #region Applying Electrify Effect
    public void ApplyElectrifyEffect(float duration, float damage, float charge) {

        float lightningResistance = _entityStats.GetElementalResistance(ElementType.Lightning);
        float finalCharge = charge * (1f - lightningResistance); // Lightning resistance reduces individual charge amount
        _currentCharge += finalCharge;

        if (_currentCharge >= _maximumCharge) {
            Instantiate(_lightningStrikeVFX, transform.position, Quaternion.identity);
            _entityHealth.ReduceHealth(damage);

            StopElectrifyEffect();
            return;
        }

        if(_electrifyCoroutine != null)
            StopCoroutine(_electrifyCoroutine);

        _electrifyCoroutine = StartCoroutine(ElectrifyEffectCo(duration));
    }

    private IEnumerator ElectrifyEffectCo(float duration) {
        _currentEffect = ElementType.Lightning;
        _entityVFX.PlayOnStatusVFX(duration, _currentEffect);

        yield return new WaitForSeconds(duration);
        StopElectrifyEffect();
    }

    private void StopElectrifyEffect() {
        _currentEffect = ElementType.None;
        _currentCharge = 0;
        _entityVFX.StopAllVFX();
    }

    #endregion

    public bool CanStatusEffectBeApplied(ElementType elementType) {

        // This is so that lightning can be applied one after another for build up
        if(elementType == ElementType.Lightning && _currentEffect == ElementType.Lightning) 
            return true;

        return _currentEffect == ElementType.None;
    }
}
