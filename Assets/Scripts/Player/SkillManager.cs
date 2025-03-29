using UnityEngine;

public class SkillManager : MonoBehaviour
{
    private static SkillManager _instance;
    public static SkillManager Instance { get { return _instance; } }
    public Player player;

    #region Skills
    public DashSkill dashSkill { get; private set; }
    public CloneSkill cloneSkill { get; private set; }
    public SwordSkill swordSkill { get; private set; }

    #endregion

    private void Awake() {
        if (_instance != null)
            Destroy(_instance.gameObject);
        else
            _instance = this;
    }

    private void Start() {
        if(dashSkill == null) dashSkill = GetComponent<DashSkill>();
        if(cloneSkill == null) cloneSkill = GetComponent<CloneSkill>();
        if (swordSkill == null) swordSkill = GetComponent<SwordSkill>();

    }
}
