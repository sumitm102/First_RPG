using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    [Header("Major stats")]
    public Stat strength; //Each point increases damage by 1 and crit power by 1%
    public Stat agility; //Each point increases evasion by 1% and crit chance by 1%
    public Stat intelligence; //Each point increases magic damage by 1 and magic resistance by 3%
    public Stat vitality; //Each point increases health by 3 or 6 points

    [Header("Offensive stats")]
    public Stat damage;
    public Stat critChance;
    public Stat critPower; //Default value 150%. Setting it on the start method. 

    [Header("Defensive stats")]
    public Stat maxHealth;
    public Stat armor;
    public Stat evasion;

    [Space]

    [SerializeField] private int _currentHealth;
    
    protected virtual void Start()
    {
        _currentHealth = maxHealth.GetValue();  
        critPower.SetDefaultValue(150);
    }

    
    void Update()
    {
        
    }

    public virtual void InflictDamage(CharacterStats _targetStats) {
        if (TargetCanAvoidAttack(_targetStats))
            return; //To prevent taking any damage after successfully evading 

        int totalDamage = damage.GetValue() + strength.GetValue();

        if (CanCrit())
            totalDamage = CalculateCritPower(totalDamage);

        totalDamage = CheckTargetArmor(_targetStats, totalDamage);

        _targetStats.TakeDamage(totalDamage);
    }
    private bool TargetCanAvoidAttack(CharacterStats _targetStats) {
        int totalEvasion = _targetStats.evasion.GetValue() + _targetStats.agility.GetValue();

        if (Random.Range(0, 100) < totalEvasion) {
            //Debug.Log("Attack avoided");

            return true;
        }

        return false;
    }

    private int CheckTargetArmor(CharacterStats _targetStats, int _totalDamage) {
        _totalDamage -= _targetStats.armor.GetValue();
        _totalDamage = Mathf.Clamp(_totalDamage, 0, int.MaxValue);

        return _totalDamage;
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

    private bool CanCrit() {
        int totalCritChance = critChance.GetValue() + agility.GetValue();

        if(Random.Range(0, 100) <= totalCritChance) 
            return true;
        
        return false;
    } 

    private int CalculateCritPower(int _damage) {
        float totalCritPower = (critPower.GetValue() + strength.GetValue()) * 0.01f; //Converting to percentage
        //Debug.Log("Total crit power %: " +  totalCritPower);

        float critDamage = _damage * totalCritPower;
        //Debug.Log("Crit damage before round up " + critDamage);

        return Mathf.RoundToInt(critDamage);
    }
}
