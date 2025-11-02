using UnityEngine;

public class SkillShard : SkillBase
{
    [SerializeField] private GameObject _shardPrefab;
    [SerializeField] private float _detonationTime = 2f;

    public void CreateShard() {
        GameObject shardSkill = Instantiate(_shardPrefab, transform.position, Quaternion.identity);

        if(shardSkill.TryGetComponent<SkillObjectShard>(out var shardObject)) {
            shardObject.SetupShard(_detonationTime);
        }
    }
}
