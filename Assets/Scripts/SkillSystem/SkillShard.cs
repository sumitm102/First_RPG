using System;
using System.Collections;
using UnityEngine;

public class SkillShard : SkillBase
{
    [SerializeField] private GameObject _shardPrefab;
    [SerializeField] private float _shardDefaultDuration = 2f; // Can also be considered the default duration time of a shard

    private SkillObjectShard _currentShard;
    private EntityHealth _playerHealth;

    [Header("Moving shard details")]
    [SerializeField] private float _shardSpeed = 7f;

    [Header("Multicast shard details")]
    [SerializeField] private int _maxCharges = 3;
    [SerializeField] private int _currentCharges;
    [SerializeField] private bool _isRecharging;

    [Header("Teleport shard details")]
    [SerializeField] private float _shardExtendedDuration = 10f; // This is the updated duration time when teleportaion in unlocked

    [Header("HP rewind shard details")]
    [SerializeField] private float _savedHPPercent;



    protected override void Awake() {
        base.Awake();

        _currentCharges = _maxCharges;
        _playerHealth = GetComponentInParent<EntityHealth>();
    }

    public void CreateShard() {
        GameObject shardSkill = Instantiate(_shardPrefab, transform.position, Quaternion.identity);

        if(shardSkill.TryGetComponent<SkillObjectShard>(out var shardObject)) {
            _currentShard = shardObject;
            _currentShard.SetupShard(this);
        }


        if (IsUpgradeUnlocked(E_SkillUpgradeType.Shard_Teleport) ||
           IsUpgradeUnlocked(E_SkillUpgradeType.Shard_TeleportHPRewind))
            _currentShard.OnExplode += ForceCooldown;
    }

    public override void TryUseSkill() {
        if (!CanUseSkill())
            return;

        if (IsUpgradeUnlocked(E_SkillUpgradeType.Shard))
            HandleShardRegular();
        else if (IsUpgradeUnlocked(E_SkillUpgradeType.Shard_MoveToEnemy))
            HandleShardMoving();
        else if (IsUpgradeUnlocked(E_SkillUpgradeType.Shard_Multicast))
            HandleShardMulticast();
        else if (IsUpgradeUnlocked(E_SkillUpgradeType.Shard_Teleport))
            HandleShardTeleport();
        else if (IsUpgradeUnlocked(E_SkillUpgradeType.Shard_TeleportHPRewind))
            HandleShardHPRewind();

    }

    #region Shard Regular

    private void HandleShardRegular() {
        CreateShard();
        SetSkillOnCooldown();
    }

    #endregion

    #region Shard Moving

    private void HandleShardMoving() {
        CreateShard();
        _currentShard.MoveTowardsClosestTarget(_shardSpeed);

        SetSkillOnCooldown();
    }

    #endregion

    #region Shard Multicast

    private void HandleShardMulticast() {
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

    #endregion

    #region Shard Teleport

    private void HandleShardTeleport() {
        if (_currentShard == null)
            CreateShard();
        else {
            SwapPlayerAndShard();
            SetSkillOnCooldown();
        }

    }

    #endregion

    #region Shard HP Rewind

    private void HandleShardHPRewind() {
        if (_currentShard == null) {
            CreateShard();
            _savedHPPercent = _playerHealth.GetHPPercent();
        }
        else {
            SwapPlayerAndShard();
            _playerHealth.SetHPToPercent(_savedHPPercent);
            SetSkillOnCooldown();
        }
    }

    #endregion


    public float GetShardDuration() {
        if (IsUpgradeUnlocked(E_SkillUpgradeType.Shard_Teleport) || 
            IsUpgradeUnlocked(E_SkillUpgradeType.Shard_TeleportHPRewind))
            return _shardExtendedDuration;

        return _shardDefaultDuration;
    }

    private void SwapPlayerAndShard() {
        Vector3 shardPosition = _currentShard.transform.position;
        Vector3 playerPosition = Player.transform.position;

        _currentShard.transform.position = playerPosition;
        _currentShard.Explode();

        Player.TeleportPlayer(shardPosition);
    }


    private void ForceCooldown() {

        if (!IsSkillOnCooldown()) {
            SetSkillOnCooldown();
            _currentShard.OnExplode -= ForceCooldown;
        }
    }


}
