using UnityEngine;

public class UITooltip : MonoBehaviour
{
    private RectTransform _rect;
    [SerializeField] private Vector2 _offset = new Vector2(300f, 20f);


    protected virtual void Awake() {
        _rect = GetComponent<RectTransform>();
    }

    public virtual void ShowTooltip(bool show, RectTransform targetRect) {
        if (!show) {
            _rect.position = new Vector2(9999f, 9999f);
            return;
        }

        UpdatePosition(targetRect);
    }


    private void UpdatePosition(RectTransform targetRect) {
        float screenCenterX = Screen.width / 2f;
        float screenTop = Screen.height;
        float screenBottom = 0f;

        Vector2 targetPosition = targetRect.position;

        // Moving the rect to slightly to the left or right depending on if it's one the left or right side of the screen
        targetPosition.x = targetPosition.x > screenCenterX ? targetPosition.x - _offset.x : targetPosition.x + _offset.x;

        float verticalHalf = _rect.sizeDelta.y / 2f;
        float topY = targetPosition.y + verticalHalf;
        float bottomY = targetPosition.y - verticalHalf;

        if (topY > screenTop)
            targetPosition.y = screenTop - verticalHalf - _offset.y;
        else if(bottomY < screenBottom)
            targetPosition.y = screenBottom + verticalHalf + _offset.y;

        _rect.position = targetPosition;
    }

    protected virtual string GetColoredText(string color, string text) {
        return $"<color={color}>{text}</color>";
    }
}
