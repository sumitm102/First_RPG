using UnityEngine;

public class SkillManager : MonoBehaviour
{
    private static SkillManager _instance;
    public static SkillManager Instance { get { return _instance; } }
    public Player player;
    public DashSkill dashSkill { get; private set; }
    public CloneSkill cloneSkill { get; private set; }

    private void Awake() {
        if (_instance != null)
            Destroy(_instance.gameObject);
        else
            _instance = this;
    }

    private void Start() {
        if(dashSkill == null) dashSkill = GetComponent<DashSkill>();
        if(cloneSkill == null) cloneSkill = GetComponent<CloneSkill>();
    }
}
