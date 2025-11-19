using System.Runtime.CompilerServices;
using UnityEngine;

public class SkillSwordThrow : SkillBase
{
    private SkillObjectSword _currentSword;

    [Header("Regular sword upgrade")]
    [SerializeField] private GameObject _swordPrefab;
    [Range(0, 10)]
    [SerializeField] private float _throwForce = 5f;

    [Header("Pierce sowrd upgrade")]
    [SerializeField] private GameObject _pierceSwordPrefab;
    public int amountToPierce = 2; 

    [Header("Spin sowrd upgrade")]
    [SerializeField] private GameObject _spinSwordPrefab;
    public int maxDistance = 5;
    public float maxSpinDuratoin = 3f;
    public float attacksPerSecond = 3f;

    [Header("Trajectory prediction details")]
    [SerializeField] private GameObject _predictionDot;
    [SerializeField] private int _numberOfDots = 20;
    [SerializeField] private float _spaceBetweenDots = 0.05f;
    private float _swordGravity;
    private Transform[] _dots;
    private Vector2 _confirmedDirection;


    protected override void Awake() {
        base.Awake();

        _swordGravity = _swordPrefab.GetComponent<Rigidbody2D>().gravityScale;
        _dots = GenerateDots();
    }

    public override bool CanUseSkill() {
        if (_currentSword != null) {
            _currentSword.GetSwordBackToPlayer();
            return false;
        }

        return base.CanUseSkill(); 
    }

    public void ThrowSword() {
        GameObject swordPrefab = GetSwordPrefab();
        GameObject newSword = Instantiate(swordPrefab, _dots[1].position, Quaternion.identity);

        _currentSword = newSword.GetComponent<SkillObjectSword>();
        _currentSword.SetupSword(this, GetThrowPower());
    }

    private GameObject GetSwordPrefab() {
        if (IsUpgradeUnlocked(E_SkillUpgradeType.SwordThrow))
            return _swordPrefab;
        else if (IsUpgradeUnlocked(E_SkillUpgradeType.SwordThrowPierce))
            return _pierceSwordPrefab;
        else if (IsUpgradeUnlocked(E_SkillUpgradeType.SwordThrowSpin))
            return _spinSwordPrefab;

        Debug.Log("No valid sword upgrade type selected");
        return null;
        
    }

    private Vector2 GetThrowPower() => _confirmedDirection * (_throwForce * 10);


    public void PredictTrajectory(Vector2 direction) {
        for(int i = 0; i < _dots.Length; i++) 
            _dots[i].position = GetTrajectoryPoint(direction, i * _spaceBetweenDots);
        
    }

    private Vector2 GetTrajectoryPoint(Vector2 direction, float t) {
        float scaledThrowPower = _throwForce * 10f;

        Vector2 initialVelocity = direction * scaledThrowPower;

        Vector2 gravityEffect = 0.5f * Physics2D.gravity * _swordGravity * (t * t);

        Vector2 predictionPoint = (initialVelocity * t) + gravityEffect;

        Vector2 playerPosition = transform.root.position;

        return playerPosition + predictionPoint;
    }

    public void EnableDots(bool enable) {
        foreach(var dot in _dots)
            dot.gameObject.SetActive(enable);
    }

    public void ConfirmTrajectory(Vector2 direction) => _confirmedDirection = direction;

    private Transform[] GenerateDots() {
        Transform[] newDots = new Transform[_numberOfDots];

        for(int i = 0; i < _numberOfDots; i++) {
            newDots[i] = Instantiate(_predictionDot, transform.position, Quaternion.identity, transform).transform;
            newDots[i].gameObject.SetActive(false);
        }

        return newDots;
    }
 }
