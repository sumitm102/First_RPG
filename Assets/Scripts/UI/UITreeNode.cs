using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UITreeNode : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    private UI _ui;
    private RectTransform _rect;
    private UISkillTree _skillTree;
    private UITreeConnectionHandler _connectionHandler;

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
        _connectionHandler = GetComponent<UITreeConnectionHandler>();

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
        _connectionHandler.ConnectionImageUnlocked(true);
    }

    public void RefundPoints() {
        isUnlocked = false;
        isLocked = false;

        UpdateIconColor(GetColorByHex(_lockedColorHex));

        _skillTree.AddSkillPoints(skillData.cost);
        _connectionHandler.ConnectionImageUnlocked(false);
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
        else if (isLocked)
            _ui.skillTooltip.TextBlinkEffect();
        else
            Debug.Log("Cannot be unlocked");
    }

    public void OnPointerEnter(PointerEventData eventData) {
        _ui.skillTooltip.ShowTooltip(true, _rect, this);

        if (isUnlocked || isLocked)
            return;
        
        Color color = Color.white * 0.9f;
        color.a = 1f;

        UpdateIconColor(color);
        
    }

    public void OnPointerExit(PointerEventData eventData) {
        _ui.skillTooltip.ShowTooltip(false, _rect);

        if (isUnlocked || isLocked)
            return;


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
