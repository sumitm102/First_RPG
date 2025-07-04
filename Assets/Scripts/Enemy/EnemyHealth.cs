using UnityEngine;

public class EnemyHealth : EntityHealth
{

    private Enemy _enemy;

    protected override void Awake() {
        base.Awake();

        if(_enemy == null)
            _enemy = GetComponent<Enemy>();
    }

    public override void TakeDamage(int damage, Transform damageDealer) {
        base.TakeDamage(damage, damageDealer);

        if (isDead)
            return;

        if (damageDealer.CompareTag("Player"))
            _enemy.TryEnterBattleState(damageDealer);
        

    }
}
