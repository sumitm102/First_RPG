using System;
using UnityEngine;

public class EntityHealth : MonoBehaviour
{
    [SerializeField] protected int maxHP = 100;
    [SerializeField] protected bool isDead;

    public virtual void TakeDamage(int damage, Transform damageDealer) {
        if (isDead)
            return;

        ReduceHP(damage);
    }

    protected void ReduceHP(int damage) {
        if (maxHP <= 0) {
            Die();
            return;
        }


        maxHP -= damage;

    }

    private void Die() {
        isDead = true;
        Debug.Log(this.gameObject.name + " Died.");
    }
}
