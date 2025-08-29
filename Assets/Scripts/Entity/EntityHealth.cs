using System;
using UnityEngine;
using UnityEngine.UI;

public class EntityHealth : MonoBehaviour, IDamagable
{
    private Entity _entity;
    private EntityVFX _entityVFX;
    private EntityStats _entityStats;

    [SerializeField] protected int currentHP;
    [SerializeField] protected bool isDead;

    [Header("On Damage Knockback")]
    [SerializeField] private Vector2 _onDamageKnockback = new Vector2(1.5f, 2.5f);
    [SerializeField] private float _knockbackDuration = 0.2f;

    [Header("On Heavy Damage Knockback")]
    [SerializeField] private Vector2 _onHeavyDamageKnockback = new Vector2(7f, 7f);
    [SerializeField] private float _heavyKnockbackDuration = 0.5f;
    [SerializeField, Range(0, 1)] private float _heavyDamageThreshold = 0.3f; // percentage of maxHP character will loose to get heavy knockback

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

        currentHP = _entityStats.GetMaxHealth();
        UpdateHealthBar();
    }

    public virtual void TakeDamage(int damage, Transform damageDealer) {
        if (isDead)
            return;

        Vector2 knockbackVelocity = CalculateKnockbackVelocity(damage, damageDealer);
        float knockbackDuration = CalculateKnockbackDuration(damage);

        if (_entityVFX != null)
            _entityVFX.PlayOnDamageVFX();

        if (_entity != null)
            _entity.ReceiveKnockback(knockbackVelocity, knockbackDuration);


        ReduceHP(damage);
    }

    protected void ReduceHP(int damage) {
        currentHP -= damage;
        UpdateHealthBar();

        if (currentHP <= 0)
            Die();
    }

    private void Die() {
        isDead = true;
        _entity.TryEnterDeadState();
    }

    private void UpdateHealthBar() {
        if (_heathBar == null)
            return;

        _heathBar.value = (currentHP * 1f) / _entityStats.GetMaxHealth();
    }

    private Vector2 CalculateKnockbackVelocity(float damage, Transform damageDealer) {

        int direction = transform.position.x > damageDealer.position.x ? 1 : -1;

        Vector2 knockbackVelocity = IsHeaveDamage(damage) ? _onHeavyDamageKnockback : _onDamageKnockback;
        knockbackVelocity.x *= direction;

        return knockbackVelocity;
    }
    private bool IsHeaveDamage(float damage) => damage / _entityStats.GetMaxHealth() > _heavyDamageThreshold;

    private float CalculateKnockbackDuration(float damage) => IsHeaveDamage(damage) ? _heavyKnockbackDuration : _knockbackDuration;

}
