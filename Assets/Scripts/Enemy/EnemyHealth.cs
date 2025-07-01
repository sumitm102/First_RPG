using UnityEngine;

public class EnemyHealth : EntityHealth
{

    private Enemy enemy;

    private void Awake() {
        enemy = GetComponent<Enemy>();
    }

    public override void TakeDamage(int damage, Transform damageDealer) {

        if (damageDealer.CompareTag("Player"))
            enemy.TryEnterBattleState(damageDealer);
        

        base.TakeDamage(damage, damageDealer);
    }
}
