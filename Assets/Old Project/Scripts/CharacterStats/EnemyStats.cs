using UnityEngine;

public class EnemyStats : CharacterStats
{
    private Enemy _enemy;


    protected override void Start() {
        base.Start();

        _enemy = GetComponent<Enemy>();
    }

    public override void TakeDamage(int _damage) {
        base.TakeDamage(_damage);

        _enemy.DamageEffect();
    }

    protected override void Die() {
        base.Die();

        _enemy.ChangeToDeadState();
    }
}
