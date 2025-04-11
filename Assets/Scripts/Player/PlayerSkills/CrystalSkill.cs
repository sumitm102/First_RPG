using System.Collections.Generic;
using UnityEngine;

public class CrystalSkill : Skill
{
    [SerializeField] private GameObject _crystalPrefab;
    [SerializeField] private float _crystalDuration;
    private GameObject _currentCrystal;
    
    public override void UseSkill() {
        base.UseSkill();

        if(_currentCrystal == null) {
            _currentCrystal = Instantiate(_crystalPrefab, player.transform.position, Quaternion.identity);
            if (_currentCrystal.TryGetComponent<CrystalSkillController>(out CrystalSkillController crystalController))
                crystalController.SetupCrystal(_crystalDuration);
        }
            
        else {

            // For teleportation
            player.transform.position = _currentCrystal.transform.position;
            Destroy(_currentCrystal);
        }
    }
}
