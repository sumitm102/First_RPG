using UnityEngine;

public class SwordSkill : Skill
{
    [Header("Skill info")]
    [SerializeField] private GameObject _swordPrefab;
    [SerializeField] private Vector2 _launchDir;
    [SerializeField] private float _swordGravity;


    public void CreateSword() {
        GameObject newSword = Instantiate(_swordPrefab, player.transform.position, transform.rotation);

        SwordSkillController newSwordSkillController = newSword.GetComponent<SwordSkillController>();

        newSwordSkillController.SetupSword(_launchDir, _swordGravity);
    }
}
