using UnityEngine;

public class EnemyVFX : EntityVFX
{
    [Header("Counter Attack Window")]
    [SerializeField] private GameObject _attackAlert;


    public void EnableAttackAlert(bool enable) {

        if (_attackAlert == null)
            return;

        _attackAlert.SetActive(enable);
    }
    
}
