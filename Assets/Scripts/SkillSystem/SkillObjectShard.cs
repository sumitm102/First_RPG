using System;
using UnityEngine;

public class SkillObjectShard : SkillObjectBase
{
    [SerializeField] private GameObject _vfxPrefab;

    private Transform _target;
    private float _speed;

    public event Action OnExplode;

    private void Update() {
        if (_target == null)
            return;

        transform.position = Vector3.MoveTowards(transform.position, _target.position, _speed * Time.deltaTime);
    }

    public void MoveTowardsClosestTarget(float speed) {
        _target = FindClosestTarget();
        _speed = speed;
    }

    public void SetupShard(float detonationTime) {

        // After some time execute the function to automatically destroy the shard
        Invoke(nameof(Explode), detonationTime);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.TryGetComponent<Enemy>(out var enemy)) {
            Explode();
        }
    }

    public void Explode() {
        DamageEnemiesInRadius(transform, checkRadius);
        Instantiate(_vfxPrefab, transform.position, Quaternion.identity);

        OnExplode?.Invoke();
        Destroy(this.gameObject);
    }
}
