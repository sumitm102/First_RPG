using System.Collections.Generic;
using UnityEngine;

public class CrystalSkill : Skill
{
    [SerializeField] private GameObject _crystalPrefab;
    [SerializeField] private float _crystalDuration;
    [SerializeField] private float _enemyCheckRadius;
    private GameObject _currentCrystal;

    [Header("Crystal mirage")]
    [SerializeField] private bool _cloneInsteadOfCrystal;

    [Header("Explosive crystal")]
    [SerializeField] private bool _canExplode;
    [SerializeField] private float _growSpeed = 5f;

    [Header("Moving crystal")]
    [SerializeField] private bool _canMoveToEnemy;
    [SerializeField] private float _moveSpeed;

    [Header("Multi stacking crystal")]
    [SerializeField] private bool _canUseMultiStacks;
    [SerializeField] private int _stackAmount;
    [SerializeField] private float _multiStackCooldown;
    [SerializeField] private float _useTimeWindow;
    [SerializeField] private List<GameObject> _crystalList = new List<GameObject>();
    
    public override void UseSkill() {
        base.UseSkill();

        if (CanUseMultiCrystal())
            return;

        if(_currentCrystal == null) 
            CreateCrystal();

        else {

            //Stop teleportation when crystal is moving
            if (_canMoveToEnemy) return;

            // For teleportation
            Vector2 playerPosition = player.transform.position;
            player.transform.position = _currentCrystal.transform.position;
            _currentCrystal.transform.position = playerPosition;

            //If true then spawn a clone at the crystal's position, otherwise make it explode or destroy it
            if (_cloneInsteadOfCrystal) {
                player.skillManager.cloneSkill.CreateClone(_currentCrystal.transform, Vector3.zero);
                Destroy(_currentCrystal);
            }
            else {

                if (_currentCrystal.TryGetComponent<CrystalSkillController>(out CrystalSkillController crystalSkillController))
                    crystalSkillController.ExplodeOrSelfDestroy();
            }
        }
    }

    public void CreateCrystal() {
        _currentCrystal = Instantiate(_crystalPrefab, player.transform.position, Quaternion.identity);

        if (_currentCrystal.TryGetComponent<CrystalSkillController>(out CrystalSkillController crystalController)) {
            crystalController.
                SetupCrystal(_crystalDuration, _canExplode, _canMoveToEnemy, _moveSpeed, _growSpeed, FindClosestEnemy(_currentCrystal.transform, _enemyCheckRadius));

            //crystalController.ChooseRandomEnemy();

        }
    }

    public void CurrentCrystalChooseRandomTarget() => _currentCrystal.GetComponent<CrystalSkillController>().ChooseRandomEnemy();

    private bool CanUseMultiCrystal() {
        if (_canUseMultiStacks && _crystalList.Count > 0) {

            if(_crystalList.Count == _stackAmount) 
                Invoke(nameof(ResetAbility), _useTimeWindow);
            

            cooldown = 0;
            GameObject crystalToSpawn = _crystalList[_crystalList.Count - 1];
            GameObject newCrystal = Instantiate(crystalToSpawn, player.transform.position, Quaternion.identity);

            _crystalList.Remove(crystalToSpawn);

            if (newCrystal.TryGetComponent<CrystalSkillController>(out CrystalSkillController crystalController))
                crystalController.
                    SetupCrystal(_crystalDuration, _canExplode, _canMoveToEnemy, _moveSpeed, _growSpeed, FindClosestEnemy(newCrystal.transform, _enemyCheckRadius));


            if (_crystalList.Count <= 0) {
                cooldown = _multiStackCooldown;
                RefillCrystalList();
            }

            return true;
        }

        return false;
    }

    private void RefillCrystalList() {
        int amountToAdd = _stackAmount - _crystalList.Count;

        for(int i = 0; i < amountToAdd; i++) {
            _crystalList.Add(_crystalPrefab);
        }
    }

    private void ResetAbility() {
        if (cooldownTimer > 0) return;

        cooldownTimer = _multiStackCooldown;
        RefillCrystalList();
    }
}
