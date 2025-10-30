using System.Collections;
using System.Text;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class UISkillTooltip : UITooltip
{
    private UI _ui;
    private UISkillTree _skillTree;

    [SerializeField] private TextMeshProUGUI _skillName;
    [SerializeField] private TextMeshProUGUI _skillDescription;
    [SerializeField] private TextMeshProUGUI _skillRequirements;
    [Space]
    [SerializeField] private string _metRequirementHex;
    [SerializeField] private string _notMetRequirementHex;
    [SerializeField] private string _importantInfoHex;
    [SerializeField] private Color _exampleColor;
    [SerializeField] private string _skillLockedText = "You've taken a different path - this skill is now locked.";

    private Coroutine _textEffectCo;


    protected override void Awake() {
        base.Awake();

        _ui = GetComponentInParent<UI>();
        _skillTree = _ui.GetComponentInChildren<UISkillTree>(true);
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

        string skillLockedText = GetColoredText(_importantInfoHex, this._skillLockedText);
        string requirementsText = treeNode.isLocked ? skillLockedText : GetRequirements(treeNode.skillData.cost, treeNode.neededNodes, treeNode.conflictingNodes);
        _skillRequirements.text = requirementsText;
    }


    private string GetRequirements(int skillCost, UITreeNode[] neededNodes, UITreeNode[] conflictingNodes) {
        StringBuilder sb = new StringBuilder();

        sb.AppendLine("Requirements:");

        string costColor = _skillTree.HasEnoughSkillPoints(skillCost) ? _metRequirementHex : _notMetRequirementHex;
        string costText = $"- {skillCost} skill {(skillCost > 1 ? "points" : "point")}";
        string finalCostText = GetColoredText(costColor, costText);

        sb.AppendLine(finalCostText);

        foreach(var node in neededNodes) {
            string nodeColor = node.isUnlocked ? _metRequirementHex : _notMetRequirementHex;
            string nodeText = $"- {node.skillData.skillName}";
            string finalNodeText = GetColoredText(nodeColor, nodeText);

            sb.AppendLine(finalNodeText);
        }

        if(conflictingNodes.Length <= 0)
            return sb.ToString();

        sb.AppendLine(); // Spacing
        sb.AppendLine(GetColoredText(_importantInfoHex, "Locks out: "));

        foreach (var node in conflictingNodes) {
            string nodeText = $"- {node.skillData.skillName}";
            string finalNodeText = GetColoredText(_importantInfoHex, nodeText);

            sb.AppendLine(finalNodeText);
        }

        return sb.ToString();

        
    }

    public void TextBlinkEffect() {
        if (_textEffectCo != null)
            StopCoroutine(_textEffectCo);

        _textEffectCo = StartCoroutine(TextBlinkEffectCo(_skillRequirements, 0.15f, 3));
    }

    private IEnumerator TextBlinkEffectCo(TextMeshProUGUI text, float blinkInterval, int blinkCount) {
        for (int i = 0; i < blinkCount; i++) {
            text.text = GetColoredText(_notMetRequirementHex, _skillLockedText);
            yield return new WaitForSeconds(blinkInterval);

            text.text = GetColoredText(_importantInfoHex, _skillLockedText);
            yield return new WaitForSeconds(blinkInterval);
        }
    }
}
