using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UITreeNode : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    private UI _ui;
    private RectTransform _rect;
    private UISkillTree _skillTree;

    [Header("Skill details")]
    public SO_SkillData skillData;
    [SerializeField] private string _skillName;
    [SerializeField] private Image _skillIcon;
    [SerializeField] private int _skillCost;
    [SerializeField] private string _lockedColorHex = "#AAA6A6";
    private Color _lastColor;

    [Header("Unlock details")]
    public UITreeNode[] neededNodes;
    public UITreeNode[] conflictingNodes;
    public bool isUnlocked;
    public bool isLocked;

    private void Awake() {
        _ui = GetComponentInParent<UI>();
        _rect = GetComponent<RectTransform>();
        _skillTree = GetComponentInParent<UISkillTree>();

        UpdateIconColor(GetColorByHex(_lockedColorHex));
        _lastColor = GetColorByHex(_lockedColorHex);
    }


    private bool CanSkillBeUnlocked() {
        if (isLocked || isUnlocked)
            return false;

        if (!_skillTree.HasEnoughSkillPoints(skillData.cost))
            return false;

        foreach(var node in neededNodes) {
            if(!node.isUnlocked)
                return false;
        }

        foreach (var node in conflictingNodes) {
            if(node.isUnlocked)
                return false;
        }

        return true;
    }

    private void LockConflictingNodes() {
        foreach (var node in conflictingNodes)
            node.isLocked = true;   
    }

    private void Unlock() {

        isUnlocked = true;
        UpdateIconColor(Color.white);
        _skillTree.RemoveSkillPoints(skillData.cost);
        LockConflictingNodes();


    }

    private void UpdateIconColor(Color color) {
        if (_skillIcon == null)
            return;

        _lastColor = _skillIcon.color;

        _skillIcon.color = color;
    }

    public void OnPointerDown(PointerEventData eventData) {
        if (CanSkillBeUnlocked())
            Unlock();
        else
            Debug.Log("Cannot be unlocked");
    }

    public void OnPointerEnter(PointerEventData eventData) {
        _ui.skillTooltip.ShowTooltip(true, _rect, this);

        if(!isUnlocked)
            UpdateIconColor(Color.white * 0.9f);
    }

    public void OnPointerExit(PointerEventData eventData) {
        _ui.skillTooltip.ShowTooltip(false, _rect);

        if(!isUnlocked)
            UpdateIconColor(_lastColor);
    }

    private Color GetColorByHex(string hexNumber) {

        ColorUtility.TryParseHtmlString(hexNumber, out var color);

        return color;
    }

    private void OnValidate() {
        if (skillData == null)
            return;

        _skillName = skillData.skillName;
        _skillIcon.sprite = skillData.skillIcon;
        _skillCost = skillData.cost;
        gameObject.name = "UITreeNode - " + skillData.skillName;
    }
}
