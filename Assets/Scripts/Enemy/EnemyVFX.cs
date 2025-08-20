using UnityEngine;

public class EnemyVFX : EntityVFX
{
    [Header("Counter Attack Window")]
    [SerializeField] private GameObject _attackAlert;


    public void EnableAttackAlert(bool enable) => _attackAlert.SetActive(enable);
    
}
