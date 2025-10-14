using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UITreeNode : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    [SerializeField] private SO_SkillData _skillData;
    [SerializeField] private string _skillName;

    [SerializeField] private Image _skillIcon;
    [SerializeField] private string _lockedColorHex = "#AAA6A6";
    private Color _lastColor;

    public bool isUnlocked;
    public bool isLocked;

    private void Awake() {
        UpdateIconColor(GetColorByHex(_lockedColorHex));
        _lastColor = GetColorByHex(_lockedColorHex);
    }

    private void OnValidate() {
        if (_skillData == null)
            return;

        _skillName = _skillData.skillName;
        _skillIcon.sprite = _skillData.skillIcon;
        gameObject.name = "UITreeNode - " + _skillData.skillName;
    }

    private bool CanSkillBeUnlocked() {
        if (isLocked || isUnlocked)
            return false;

        return true;
    }

    private void Unlock() {

        isUnlocked = true;

        UpdateIconColor(Color.white);
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
        if(!isUnlocked)
            UpdateIconColor(Color.white * 0.9f);
    }

    public void OnPointerExit(PointerEventData eventData) {

        if(!isUnlocked)
            UpdateIconColor(_lastColor);
    }

    private Color GetColorByHex(string hexNumber) {

        ColorUtility.TryParseHtmlString(hexNumber, out var color);

        return color;
    }
}
