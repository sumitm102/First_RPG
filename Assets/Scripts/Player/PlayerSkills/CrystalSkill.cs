using System.Collections.Generic;
using UnityEngine;

public class CrystalSkill : Skill
{
    [SerializeField] private GameObject _crystalPrefab;
    [SerializeField] private float _crystalDuration;
    [SerializeField] private float _enemyCheckRadius;
    private GameObject _currentCrystal;

    [Header("Explosive crystal")]
    [SerializeField] private bool _canExplode;
    [SerializeField] private float _growSpeed = 5f;

    [Header("Moving crystal")]
    [SerializeField] private bool _canMoveToEnemy;
    [SerializeField] private float _moveSpeed;
    
    public override void UseSkill() {
        base.UseSkill();

        if(_currentCrystal == null) {
            _currentCrystal = Instantiate(_crystalPrefab, player.transform.position, Quaternion.identity);
            if (_currentCrystal.TryGetComponent<CrystalSkillController>(out CrystalSkillController crystalController))
                crystalController.SetupCrystal(_crystalDuration, _canExplode, _canMoveToEnemy, _moveSpeed, _growSpeed, FindClosestEnemy(_currentCrystal.transform, _enemyCheckRadius));
        }
            
        else {

            //Stop teleportation when crystal is moving
            if (_canMoveToEnemy) return;

            Vector2 playerPosition = player.transform.position;

            // For teleportation
            player.transform.position = _currentCrystal.transform.position;
            _currentCrystal.transform.position = playerPosition;
            
            if(_currentCrystal.TryGetComponent<CrystalSkillController>(out CrystalSkillController crystalSkillController))
                crystalSkillController.ExplodeOrSelfDestroy();
        }
    }
}
