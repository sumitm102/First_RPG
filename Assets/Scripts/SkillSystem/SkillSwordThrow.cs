using UnityEngine;

public class SkillSwordThrow : SkillBase
{
    [Range(0, 10)]
    [SerializeField] private float _throwForce = 5f;
    [SerializeField] private float _swordGravity = 3.5f;

    [Header("Trajectory prediction details")]
    [SerializeField] private GameObject _predictionDot;
    [SerializeField] private int _numberOfDots = 20;
    [SerializeField] private float _spaceBetweenDots = 0.05f;
    private Transform[] _dots;
    private Vector2 _confirmedDirection;


    protected override void Awake() {
        base.Awake();
        _dots = GenerateDots();
    }

    public void ThrowSword() {
        Debug.Log("Sword Created");
    }

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
