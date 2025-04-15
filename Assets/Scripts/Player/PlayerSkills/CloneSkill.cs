using UnityEngine;

public class CloneSkill : Skill
{

    [Header("Clone info")]
    [SerializeField] private GameObject _clonePrefab;
    [SerializeField] private float _cloneDuration;
    [SerializeField] private bool _canAttack = true;
    [SerializeField] private float _enemyCheckRadius = 5f;

    public void CreateClone(Transform _clonePosition, Vector3 _offset) {
        GameObject newClone = Instantiate(_clonePrefab);

        //Calling the method that handles logic related to the clone itself
        if(newClone.TryGetComponent<CloneSkillController>(out CloneSkillController controller)) {
            controller.SetupClone(_clonePosition, _cloneDuration, _canAttack, _offset, FindClosestEnemy(newClone.transform, _enemyCheckRadius));
        }
    }
}
