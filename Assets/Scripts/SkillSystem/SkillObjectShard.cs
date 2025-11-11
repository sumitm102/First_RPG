using System;
using UnityEngine;

public class SkillObjectShard : SkillObjectBase
{
    [SerializeField] private GameObject _vfxPrefab;

    private Transform _target;
    private float _speed;

    public event Action OnExplode;

    private SkillShard _skillShard;

    private void Update() {
        if (_target == null)
            return;

        transform.position = Vector3.MoveTowards(transform.position, _target.position, _speed * Time.deltaTime);
    }

    public void MoveTowardsClosestTarget(float speed) {
        _target = FindClosestTarget();
        _speed = speed;
    }

    public void SetupShard(SkillShard skillShard) {
        _skillShard = skillShard;
        float duration = _skillShard.GetShardDuration();

        playerStats = _skillShard.Player.Stats;
        damageScaleData = _skillShard.DamageScaleData;

        // After some time execute the function to automatically destroy the shard
        Invoke(nameof(Explode), duration);
    }

    public void SetupShard(SkillShard skillShard, float duration, bool canMove, float shardSpeed) {
        _skillShard = skillShard;

        playerStats = skillShard.Player.Stats;
        damageScaleData = skillShard.DamageScaleData;

        // After some time execute the function to automatically destroy the shard
        Invoke(nameof(Explode), duration);

        if (canMove)
            MoveTowardsClosestTarget(shardSpeed);
        
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.TryGetComponent<Enemy>(out var enemy)) {
            Explode();
        }
    }

    public void Explode() {
        DamageEnemiesInRadius(transform, checkRadius);

        GameObject vfx = Instantiate(_vfxPrefab, transform.position, Quaternion.identity);
        SpriteRenderer sprite = vfx.GetComponentInChildren<SpriteRenderer>();
        sprite.color = _skillShard.Player.VFX.GetElementColor(usedElement);

        OnExplode?.Invoke();
        Destroy(this.gameObject);
    }
}
