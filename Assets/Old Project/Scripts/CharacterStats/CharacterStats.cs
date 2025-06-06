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

    public bool isIgnited; //Does damage over time
    public bool isChilled; //Reduce armor by 20%
    public bool isShocked; //Reduce accuracy by 20%

    [Space]

    [SerializeField] private int _currentHealth;

    private float _ignitedTimer;
    private float _chilledTimer;
    private float _shockedTimer;

    private float _igniteDamageCooldown = 0.3f;
    private float _igniteDamageTimer;
    private int _igniteDamage;
    protected virtual void Start()
    {
        _currentHealth = maxHealth.GetValue();  
        critPower.SetDefaultValue(150);
    }

    
    protected virtual void Update()
    {
        _ignitedTimer -= Time.deltaTime;
        _chilledTimer -= Time.deltaTime;
        _shockedTimer -= Time.deltaTime;

        _igniteDamageTimer -= Time.deltaTime;

        if (_ignitedTimer < 0)
            isIgnited = false;

        if (_chilledTimer < 0)
            isChilled = false;

        if(_shockedTimer < 0)
            isShocked = false;

        if(_igniteDamageTimer < 0 && isIgnited) {
            Debug.Log("Take burn damage" + _igniteDamage);

            _currentHealth -= _igniteDamage;

            if (_currentHealth < 0)
                Die();

            _igniteDamageTimer = _igniteDamageCooldown;
        }

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

        //Apply 20% of fire damage if ignited
        if (canApplyIgnite)
            _targetStats.SetupIgniteDamage(Mathf.RoundToInt(_fireDamage * 0.2f)); 

        _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
    }

    public void ApplyAilments(bool _ignited, bool _chilled, bool _shocked) {

        if (isIgnited || isChilled || isShocked)
            return;

        if (_ignited) {
            isIgnited = _ignited;
            _ignitedTimer = 2;
        }

        if (_chilled) {
            isChilled = _chilled;
            _chilledTimer = 2;
        }

        if (_shocked) {
            isShocked = _shocked;
            _shockedTimer = 2;
        }

    }

    public void SetupIgniteDamage(int _damage) {
        _igniteDamage = _damage;
    }

    private bool TargetCanAvoidAttack(CharacterStats _targetStats) {
        int totalEvasion = _targetStats.evasion.GetValue() + _targetStats.agility.GetValue();

        if (isShocked)
            totalEvasion += 20;

        if (Random.Range(0, 100) < totalEvasion) {
            //Debug.Log("Attack avoided");

            return true;
        }

        return false;
    }

    private int CheckTargetArmor(CharacterStats _targetStats, int _totalDamage) {

        if(_targetStats.isChilled)
            _totalDamage -= Mathf.RoundToInt(_targetStats.armor.GetValue() * 0.8f);
        else
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
