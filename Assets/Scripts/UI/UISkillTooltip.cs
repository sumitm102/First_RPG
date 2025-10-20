using System.Text;
using TMPro;
using UnityEngine;

public class UISkillTooltip : UITooltip
{
    private UISkillTree _skillTree;

    [SerializeField] private TextMeshProUGUI _skillName;
    [SerializeField] private TextMeshProUGUI _skillDescription;
    [SerializeField] private TextMeshProUGUI _skillRequirements;
    [Space]
    [SerializeField] private string _metRequirementHex;
    [SerializeField] private string _notMetRequirementHex;
    [SerializeField] private string _importantInfoHex;
    [SerializeField] private Color _exampleColor;
    [SerializeField] private string skillLockedText = "You've taken a different path - this skill is now locked.";


    protected override void Awake() {
        base.Awake();
        _skillTree = GetComponentInParent<UISkillTree>();
    }

    public override void ShowTooltip(bool show, RectTransform targetRect) {
        base.ShowTooltip(show, targetRect);
    }

    public void ShowTooltip(bool show, RectTransform targetRect, UITreeNode treeNode) {
        base.ShowTooltip(show, targetRect);

        if (!show)
            return;

        _skillName.text = treeNode.skillData.skillName;
        _skillDescription.text = treeNode.skillData.description;

        string skillLockedText = $"<color={_importantInfoHex}>{this.skillLockedText}</color>";
        string requirementsText = treeNode.isLocked ? skillLockedText : GetRequirements(treeNode.skillData.cost, treeNode.neededNodes, treeNode.conflictingNodes);
        _skillRequirements.text = requirementsText;
    }


    private string GetRequirements(int skillCost, UITreeNode[] neededNodes, UITreeNode[] conflictingNodes) {
        StringBuilder sb = new StringBuilder();

        sb.AppendLine("Requirements:");

        string costColor = _skillTree.HasEnoughSkillPoints(skillCost) ? _metRequirementHex : _notMetRequirementHex;
        sb.AppendLine($"<color={costColor}>- {skillCost} skill {(skillCost > 1 ? "points": "point")}</color>");

        foreach(var node in neededNodes) {
            string nodeColor = node.isUnlocked ? _metRequirementHex : _notMetRequirementHex;
            sb.AppendLine($"<color={nodeColor}>- {node.skillData.skillName}</color>");
        }

        if(conflictingNodes.Length <= 0)
            return sb.ToString();
        sb.AppendLine(); // Spacing
        sb.AppendLine($"<color={_importantInfoHex}>Locks out: </color>");

        foreach (var node in conflictingNodes) {
            sb.AppendLine($"<color={_importantInfoHex}>- {node.skillData.skillName}</color>");
        }

        return sb.ToString();

        
    }
}
