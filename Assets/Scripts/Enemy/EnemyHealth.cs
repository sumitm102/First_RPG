using UnityEngine;

public class EnemyHealth : EntityHealth
{

    private Enemy _enemy;

    protected override void Awake() {
        base.Awake();

        if(_enemy == null)
            _enemy = GetComponent<Enemy>();
    }

    public override bool TakeDamage(float damage, float elementalDamage, ElementType elementType, Transform damageDealer) {
        bool tookDamage = base.TakeDamage(damage, elementalDamage, elementType, damageDealer);

        if (!tookDamage)
            return false;

        // Commented this since both approach accomplish the same result
        //if (damageDealer.CompareTag("Player"))
        //    _enemy.TryEnterBattleState(damageDealer);

        if (damageDealer.TryGetComponent<Player>(out Player player))
            _enemy.TryEnterBattleState(damageDealer);

        return true;
        

    }
}
