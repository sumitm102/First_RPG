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
    public Stat magicResistance;

    [Header("Magic stats")]
    public Stat fireDamage;
    public Stat iceDamage;
    public Stat lightningDamage;

    public bool isIgnited;
    public bool isChilled;
    public bool isShocked;

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

        //_targetStats.TakeDamage(totalDamage);
        InflictMagicalDamage(_targetStats);
    }

    public virtual void InflictMagicalDamage(CharacterStats _targetStats) {

        int _fireDamage = fireDamage.GetValue();
        int _iceDamage = iceDamage.GetValue();
        int _lightningDamage = lightningDamage.GetValue();

        int totalMagicalDamage = _fireDamage + _iceDamage + _lightningDamage + intelligence.GetValue();
        totalMagicalDamage -= _targetStats.magicResistance.GetValue() + (_targetStats.intelligence.GetValue() * 3);
        totalMagicalDamage = Mathf.Clamp(totalMagicalDamage, 0, int.MaxValue);
        Debug.Log(totalMagicalDamage);

        _targetStats.TakeDamage(totalMagicalDamage);

        //Making sure at least one of the ailments has a value greater than 0 to avoid infinite looping
        if (Mathf.Max(_fireDamage, _iceDamage, _lightningDamage) <= 0)
            return;

        bool canApplyIgnite = _fireDamage > _iceDamage && _fireDamage > _lightningDamage;
        bool canApplyChill = _iceDamage > _fireDamage && _iceDamage > _lightningDamage;
        bool canApplyShock = _lightningDamage > _fireDamage && _lightningDamage > _iceDamage;

        //Randomly apply one of the ailments if damage amount of all ailments are equal
        while(!canApplyIgnite && !canApplyChill && !canApplyShock) {
            if(Random.value < 0.3f && _fireDamage > 0) {
                canApplyIgnite = true;
                _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
                Debug.Log("Applied fire");
                return;
            }

            if(Random.value < 0.4f && _iceDamage > 0) {
                canApplyChill = true;
                _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
                Debug.Log("Applied ice");
                return;
            }
            
            if(Random.value < 0.5f && _lightningDamage > 0) {
                canApplyShock = true;
                _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
                Debug.Log("Applied lightning");
                return;
            }
        }

        _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
    }

    public void ApplyAilments(bool _ignited, bool _chilled, bool _shocked) {

        if (isIgnited || isChilled || isShocked)
            return;

        isIgnited = _ignited;
        isChilled = _chilled;
        isShocked = _shocked;

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
