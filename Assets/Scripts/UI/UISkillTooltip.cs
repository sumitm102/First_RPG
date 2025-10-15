using TMPro;
using UnityEngine;

public class UISkillTooltip : UITooltip
{
    [SerializeField] private TextMeshProUGUI _skillName;
    [SerializeField] private TextMeshProUGUI _skillDescription;
    [SerializeField] private TextMeshProUGUI _skillRequirements;

    public override void ShowTooltip(bool show, RectTransform targetRect) {
        base.ShowTooltip(show, targetRect);
    }

    public void ShowTooltip(bool show, RectTransform targetRect, SO_SkillData skillData) {
        base.ShowTooltip(show, targetRect);

        if (!show)
            return;

        _skillName.text = skillData.skillName;
        _skillDescription.text = skillData.description;
        _skillRequirements.text = "Requirements: \n" + " - " + skillData.cost + " skill point" + (skillData.cost > 1 ? "s." : ".");
    }
}
