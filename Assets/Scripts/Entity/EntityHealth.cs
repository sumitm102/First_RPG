using System;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class EntityHealth : MonoBehaviour, IDamagable
{
    private Entity _entity;
    private EntityVFX _entityVFX;
    private EntityStats _entityStats;

    [SerializeField] protected float currentHealth;
    [SerializeField] protected bool isDead;

    [Header("On Damage Knockback")]
    [SerializeField] private Vector2 _onDamageKnockback = new Vector2(1.5f, 2.5f);
    [SerializeField] private float _knockbackDuration = 0.2f;

    [Header("On Heavy Damage Knockback")]
    [SerializeField] private Vector2 _onHeavyDamageKnockback = new Vector2(7f, 7f);
    [SerializeField] private float _heavyKnockbackDuration = 0.5f;
    [SerializeField, Range(0, 1)] private float _heavyDamageThreshold = 0.3f; // percentage of maxHP character will loose to get heavy knockback

    [Header("Health Regen")]
    [SerializeField] private float _regenInterval = 1f;
    [SerializeField] private bool _canRegenerateHealth = true;

    #region UI variables

    private Slider _heathBar;

    #endregion

    protected virtual void Awake() {

        if (_entity == null)
            _entity = GetComponent<Entity>();

        if (_entityVFX == null)
            _entityVFX = GetComponent<EntityVFX>();

        if(_entityStats == null)
            _entityStats = GetComponent<EntityStats>();

        if(_heathBar == null)
            _heathBar = GetComponentInChildren<Slider>();

        currentHealth = _entityStats.GetMaxHealth();
        UpdateHealthBar();

        InvokeRepeating(nameof(RegenerateHealth),0, _regenInterval);
    }

    public virtual bool TakeDamage(float physicalDamage, float elementalDamage, E_ElementType elementType, Transform damageDealer) {
        if (isDead)
            return false;

        if (AttackEvaded()) {
            Debug.Log($"{gameObject.name} evaded an attack");
            return false;
        }

        EntityStats damageDealerStats = damageDealer.GetComponent<EntityStats>();


        // The final physical damage taken is calculated based the damage dealer's armor reduction stat and entity's own armor mitigation stat
        float armorReduction = damageDealerStats != null ? damageDealerStats.GetArmorReduction() : 0;
        float mitigation = _entityStats.GetArmorMitigation(armorReduction);
        float physicalDamageTaken = physicalDamage * (1f - mitigation);

        // The final elemental damage taken is calculated based on the entity's own elemental resistance to the highest elemental damage dealt by the damage dealer
        float elementalResistance = _entityStats.GetElementalResistance(elementType);
        float elementalDamageTaken = elementalDamage * (1f - elementalResistance);

        TakeKnockback(damageDealer, physicalDamageTaken);
        ReduceHealth(physicalDamageTaken + elementalDamageTaken);

        return true;
    }

    private void TakeKnockback(Transform damageDealer, float finalPhysicalDamage) {
        Vector2 knockbackVelocity = CalculateKnockbackVelocity(finalPhysicalDamage, damageDealer);
        float knockbackDuration = CalculateKnockbackDuration(finalPhysicalDamage);


        if (_entity != null)
            _entity.ReceiveKnockback(knockbackVelocity, knockbackDuration);
    }

    private bool AttackEvaded() => Random.Range(0, 100f) < _entityStats.GetEvasion();
    

    public void ReduceHealth(float damage) {

        // Character flashes when taking damage
        if (_entityVFX != null)
            _entityVFX.PlayOnDamageVFX();

        currentHealth -= damage;


        UpdateHealthBar();

        if (currentHealth <= 0) {
            _canRegenerateHealth = false;
            Die();
        }
        else {
            _canRegenerateHealth = true;
        }

    
    }


    #region Regenerate Health Methods

    private void RegenerateHealth() {
        if (!_canRegenerateHealth)
            return;

        float regenAmount = _entityStats.resourceStats.healthRegen.GetValue();
        IncreaseHealth(regenAmount);
    }
    public void IncreaseHealth(float regenAmount) {

        if (isDead)
            return;

        float newHealth = currentHealth + regenAmount;
        float maxHealth = _entityStats.GetMaxHealth();

        currentHealth = Mathf.Min(newHealth, maxHealth);

        _canRegenerateHealth = currentHealth >= maxHealth ? false : true;

        UpdateHealthBar();

    }

    #endregion

    private void Die() {
        isDead = true;
        _entity.TryEnterDeadState();
    }

    private void UpdateHealthBar() {
        if (_heathBar == null)
            return;

        _heathBar.value = currentHealth / _entityStats.GetMaxHealth();
    }

    private Vector2 CalculateKnockbackVelocity(float damage, Transform damageDealer) {

        int direction = transform.position.x > damageDealer.position.x ? 1 : -1;

        Vector2 knockbackVelocity = IsHeaveDamage(damage) ? _onHeavyDamageKnockback : _onDamageKnockback;
        knockbackVelocity.x *= direction;

        return knockbackVelocity;
    }

    #region Used in skills

    public float GetHPPercent() => currentHealth / _entityStats.GetMaxHealth();

    public void SetHPToPercent(float percent) {
        currentHealth = _entityStats.GetMaxHealth() * Mathf.Clamp01(percent);
        UpdateHealthBar();
    }

    #endregion

    private bool IsHeaveDamage(float damage) => damage / _entityStats.GetMaxHealth() > _heavyDamageThreshold;

    private float CalculateKnockbackDuration(float damage) => IsHeaveDamage(damage) ? _heavyKnockbackDuration : _knockbackDuration;

}
