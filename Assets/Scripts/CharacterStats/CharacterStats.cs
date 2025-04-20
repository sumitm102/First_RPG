using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public Stat maxHealth;
    public Stat damage;
    public Stat strength;

    [SerializeField] private int _currentHealth;
    
    protected virtual void Start()
    {
        _currentHealth = maxHealth.GetValue();
    }

    
    void Update()
    {
        
    }

    public virtual void InflictDamage(CharacterStats _targetStats) {

        int totalDamage = damage.GetValue() + strength.GetValue();
        _targetStats.TakeDamage(totalDamage);
    }

    public virtual void TakeDamage(int _damage) {

        _currentHealth -= _damage;
        Debug.Log(_currentHealth);

        if(_currentHealth <= 0) {
            Die();
        }

    }

    protected virtual void Die() {

    }
}
