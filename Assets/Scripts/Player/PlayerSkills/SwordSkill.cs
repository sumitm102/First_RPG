using System;
using UnityEngine;

public enum SwordType {
    Regular,
    Bounce,
    Pierce,
    Spinning
}

public class SwordSkill : Skill
{
    //Default sword type
    public SwordType swordType = SwordType.Regular;

    [Header("Bouncy info")]
    [SerializeField] private int _bounceAmount;
    [SerializeField] private float _bounceGravity;
    [SerializeField] private float _boundSpeed;

    [Header("Pierce info")]
    [SerializeField] private int _pierceAmount;
    [SerializeField] private float _pierceGravity;

    [Header("Spin info")]
    [SerializeField] private float _damageCooldown = 0.35f;
    [SerializeField] private float _maxTravelDistance = 7f;
    [SerializeField] private float _spinDuration = 2f;
    [SerializeField] private float _spinGravity = 1f;


    [Header("Skill info")]
    [SerializeField] private GameObject _swordPrefab;
    [SerializeField] private Vector2 _launchForce;
    [SerializeField] private float _swordGravity;
    [SerializeField] private float _freezeTimeDuration;
    [SerializeField] private float _returnSpeed;

    private Vector2 _finalDir;

    [Header("Aim dots")]
    [SerializeField] private int _numOfDots;
    [SerializeField] private float _spaceBetweenDots;
    [SerializeField] private GameObject _dotPrefab;
    [SerializeField] private Transform _dotsParent;

    private GameObject[] _dots;


    protected override void Start() {
        base.Start();

        GenerateDots();

        SetupGravity();
    }

    private void SetupGravity() {
        if (swordType == SwordType.Bounce)
            _swordGravity = _bounceGravity;
        else if (swordType == SwordType.Pierce)
            _swordGravity = _pierceGravity;
        else if(swordType == SwordType.Spinning)
            _swordGravity = _spinGravity;
    }

    protected override void Update() {
        base.Update();


        //Attempts to throw sword at the cursor's position when right mouse click is released
        if (Input.GetMouseButtonUp(1)) {
            _finalDir = new Vector2(AimDirection().x * _launchForce.x, AimDirection().y * _launchForce.y);
        }

        if (Input.GetMouseButton(1)) {
            for(int i = 0; i < _dots.Length; i++) {
                _dots[i].transform.position = DotsPosition(i * _spaceBetweenDots);
            }
        }
    }

    public void CreateSword() {
        GameObject newSword = Instantiate(_swordPrefab, player.transform.position, transform.rotation);

        //SwordSkillController newSwordSkillController = newSword.GetComponent<SwordSkillController>();

        if(newSword.TryGetComponent<SwordSkillController>(out SwordSkillController newSwordSkillController)) {
            if (swordType == SwordType.Bounce)
                newSwordSkillController.SetupBounce(true, _bounceAmount, _boundSpeed);
            else if (swordType == SwordType.Pierce)
                newSwordSkillController.SetupPierce(_pierceAmount);
            else if (swordType == SwordType.Spinning)
                newSwordSkillController.SetupSpin(true, _maxTravelDistance, _spinDuration, _damageCooldown);

            newSwordSkillController.SetupSword(_finalDir, _swordGravity, player, _freezeTimeDuration, _returnSpeed);
        }

      
        player.AssignNewSword(newSword);

        DotsActive(false);
    }



    #region Aiming and Dots
    public Vector2 AimDirection() {
        Vector2 playerPosition = player.transform.position;
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePosition - playerPosition;

        return direction.normalized;
    }


    private void GenerateDots() {
        _dots = new GameObject[_numOfDots];

        for(int i = 0; i < _numOfDots; i++) {
            _dots[i] = Instantiate(_dotPrefab, player.transform.position, Quaternion.identity, _dotsParent);

            //Stays inactive by default
            _dots[i].SetActive(false);
        }
    }

    public void DotsActive(bool _isActive) {
        for(int i = 0; i < _dots.Length; i++)
            _dots[i].SetActive(_isActive);
    }

    private Vector2 DotsPosition(float t) {
        Vector2 position = (Vector2)player.transform.position + new Vector2(
            AimDirection().x * _launchForce.x,
            AimDirection().y * _launchForce.y) * t + 0.5f * (Physics2D.gravity * _swordGravity) * (t * t);

        return position;
    }

    #endregion

}
