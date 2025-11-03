using UnityEngine;

public class UISkillTree : MonoBehaviour
{
    [SerializeField] private int _skillPoints;
    [SerializeField] private UITreeConnectionHandler[] _parentNodes;

    public PlayerSkillManager PlayerSkillManager { get; private set; }

    public void Awake() {
        PlayerSkillManager = FindFirstObjectByType<PlayerSkillManager>();
    }
    private void Start() {
        UpdateAllConnections(); 
    }

    public bool HasEnoughSkillPoints(int cost) {
        return _skillPoints >= cost;
    }

    public void AddSkillPoints(int cost) {
        _skillPoints += cost;
    }

    public void RemoveSkillPoints(int cost) {
        _skillPoints -= cost;
    }

    [ContextMenu("Reset Skill Tree")]
    public void RefundAllSkills() {
        UITreeNode[] skillNodes = GetComponentsInChildren<UITreeNode>();

        foreach (var node in skillNodes)
            node.RefundPoints();
    }

    [ContextMenu("Update All Connections")]
    public void UpdateAllConnections() {
        foreach(var node in _parentNodes) {
            node.UpdateAllChildConnections();
        }
    }
}
