using UnityEngine;

public class UI : MonoBehaviour
{
    public UISkillTooltip skillTooltip;

    private void Awake() {
        skillTooltip = GetComponentInChildren<UISkillTooltip>();
    }
}
