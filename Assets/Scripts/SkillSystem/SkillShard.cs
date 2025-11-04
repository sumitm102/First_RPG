using System;
using System.Collections;
using UnityEngine;

public class SkillShard : SkillBase
{
    [SerializeField] private GameObject _shardPrefab;
    [SerializeField] private float _detonationTime = 2f;

    private SkillObjectShard _currentShard;

    [Header("Moving shard details")]
    [SerializeField] private float _shardSpeed = 7f;

    [Header("Multicast shard details")]
    [SerializeField] private int _maxCharges = 3;
    [SerializeField] private int _currentCharges;
    [SerializeField] private bool _isRecharging;

    protected override void Awake() {
        base.Awake();

        _currentCharges = _maxCharges;
    }

    public void CreateShard() {
        GameObject shardSkill = Instantiate(_shardPrefab, transform.position, Quaternion.identity);

        if(shardSkill.TryGetComponent<SkillObjectShard>(out var shardObject)) {
            _currentShard = shardObject;
            shardObject.SetupShard(_detonationTime);
        }
    }

    public override void TryUseSkill() {
        if (!CanUseSkill())
            return;

        if (Unlocked(E_SkillUpgradeType.Shard))
            HandleShardRegular();
        else if (Unlocked(E_SkillUpgradeType.Shard_MoveToEnemy))
            HandleShardMoving();
        else if (Unlocked(E_SkillUpgradeType.Shard_Multicast))
            HandleShardMultiCast();

    }

    private void HandleShardRegular() {
        CreateShard();
        SetSkillOnCooldown();
    }

    private void HandleShardMoving() {
        CreateShard();
        _currentShard.MoveTowardsClosestTarget(_shardSpeed);

        SetSkillOnCooldown();
    }

    private void HandleShardMultiCast() {
        if (_currentCharges <= 0)
            return;

        CreateShard();
        _currentShard.MoveTowardsClosestTarget(_shardSpeed);
        _currentCharges--;

        if (!_isRecharging)
            StartCoroutine(ShardRechargeCo());
    }

    private IEnumerator ShardRechargeCo() {
        _isRecharging = true;

        while (_currentCharges < _maxCharges) {
            yield return new WaitForSeconds(cooldown);
            _currentCharges++;
        }

        _isRecharging = false;
    }
}
