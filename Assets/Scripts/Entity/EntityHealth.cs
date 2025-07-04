using System;
using UnityEngine;

public class EntityHealth : MonoBehaviour
{
    private Entity _entity;
    private EntityVFX _entityVFX;

    [SerializeField] protected int maxHP = 100;
    [SerializeField] protected int currentHP;
    [SerializeField] protected bool isDead;

    [Header("On Damage Knockback")]
    [SerializeField] private Vector2 _onDamageKnockback = new Vector2(1.5f, 2.5f);
    [SerializeField] private float _knockbackDuration = 0.2f;

    [Header("On Heavy Damage Knockback")]
    [SerializeField] private Vector2 _onHeavyDamageKnockback = new Vector2(7f, 7f);
    [SerializeField] private float _heavyKnockbackDuration = 0.5f;
    [SerializeField, Range(0, 1)] private float _heavyDamageThreshold = 0.3f; // percentage of maxHP character will loose to get heavy knockback

    protected virtual void Awake() {

        currentHP = maxHP;

        if (_entity == null)
            _entity = GetComponent<Entity>();

        if (_entityVFX == null)
            _entityVFX = GetComponent<EntityVFX>();
    }

    public virtual void TakeDamage(int damage, Transform damageDealer) {
        if (isDead)
            return;

        float knockbackDuration = CalculateKnockbackDuration(damage);

        if (_entityVFX != null)
            _entityVFX.PlayOnDamageVFX();

        if (_entity != null)
            _entity.ReceiveKnockback(CalculateKnockbackVelocity(damage, damageDealer), knockbackDuration);


        ReduceHP(damage);
    }

    protected void ReduceHP(int damage) {
        currentHP -= damage;

        if (currentHP <= 0)
            Die();
    }

    private void Die() {
        isDead = true;
        _entity.EntityDeath();
    }

    private Vector2 CalculateKnockbackVelocity(float damage, Transform damageDealer) {

        int direction = transform.position.x > damageDealer.position.x ? 1 : -1;

        Vector2 knockbackVelocity = IsHeaveDamage(damage) ? _onHeavyDamageKnockback : _onDamageKnockback;
        knockbackVelocity.x *= direction;

        return knockbackVelocity;
    }
    private bool IsHeaveDamage(float damage) => damage / maxHP > _heavyDamageThreshold;

    private float CalculateKnockbackDuration(float damage) => IsHeaveDamage(damage) ? _heavyKnockbackDuration : _knockbackDuration;

}
