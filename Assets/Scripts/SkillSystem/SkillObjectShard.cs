using UnityEngine;

public class SkillObjectShard : SkillObjectBase
{
    [SerializeField] private GameObject _vfxPrefab;

    public void SetupShard(float detonationTime) {

        // After some time execute the function to automatically destroy the shard
        Invoke(nameof(Explode), detonationTime);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.TryGetComponent<Enemy>(out var enemy)) {
            Explode();
        }
    }

    private void Explode() {
        DamageEnemiesInRadius(transform, checkRadius);
        Instantiate(_vfxPrefab, transform.position, Quaternion.identity);

        Destroy(this.gameObject);
    }
}
