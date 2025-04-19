using System.Collections;
using UnityEngine;

public class CloneSkill : Skill
{

    [Header("Clone info")]
    [SerializeField] private GameObject _clonePrefab;
    [SerializeField] private float _cloneDuration;
    [SerializeField] private bool _canAttack = true;
    [SerializeField] private float _enemyCheckRadius = 5f;
    [SerializeField] private float _chanceToDuplicate;

    [SerializeField] private bool _canCreateCloneOnDashStart;
    [SerializeField] private bool _canCreateCloneOnDashEnd;
    [SerializeField] private bool _canCreateCloneOnCounterAttack;
    [SerializeField] private bool _canDuplicateClone;

    [Header("Crystal instead of clone")]
    public bool crystalInsteadOfClone;

    public void CreateClone(Transform _clonePosition, Vector3 _offset) {

        if (crystalInsteadOfClone) {
            SkillManager.Instance.crystalSkill.CreateCrystal();
            return;
        }

        GameObject newClone = Instantiate(_clonePrefab);

        //Calling the method that handles logic related to the clone itself
        if(newClone.TryGetComponent<CloneSkillController>(out CloneSkillController controller)) {
            controller.SetupClone(_clonePosition, _cloneDuration, _canAttack, _offset, FindClosestEnemy(newClone.transform, _enemyCheckRadius), _canDuplicateClone, _chanceToDuplicate);
        }
    }

    public void CreateCloneOnDashStart() {
        if (_canCreateCloneOnDashStart) {
            CreateClone(player.transform, Vector3.zero);
        }
    }

    public void CreateCloneOnDashEnd() {
        if (_canCreateCloneOnDashEnd)
            CreateClone(player.transform, Vector3.zero);
    }

    public void CreateCloneOnCounterAttack(Transform _enemyTransform) {
        if (_canCreateCloneOnCounterAttack)
            StartCoroutine(CreateCloneWithDelay(_enemyTransform, new Vector3(2.5f * player.facingDir, 0)));
    }

    private IEnumerator CreateCloneWithDelay(Transform _transform, Vector3 _offset) {
        yield return new WaitForSeconds(0.4f);
        CreateClone(_transform, _offset);
    }
}
