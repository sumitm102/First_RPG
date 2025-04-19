using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public Stat maxHealth;
    public Stat damage;

    [SerializeField] private int _currentHealth;
    
    void Start()
    {
        _currentHealth = maxHealth.GetValue();

        //Example: Equip sword with 4 damage
        damage.AddModifier(4);
    }

    
    void Update()
    {
        
    }

    public void TakeDamage(int _damage) {

        _currentHealth -= _damage;

        if(_currentHealth <= 0) {
            Die();
        }

    }

    private void Die() {

    }
}
