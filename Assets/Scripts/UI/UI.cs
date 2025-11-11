using UnityEngine;

public class UI : MonoBehaviour
{
    public UISkillTooltip skillTooltip;
    public UISkillTree skillTree;
    private bool _isSkillTreeEnabled;

    private void Awake() {
        skillTooltip = GetComponentInChildren<UISkillTooltip>();
        skillTree = GetComponentInChildren<UISkillTree>(true); // Able to get the component even when the game object is not active
    }



    public void ToggleSkillTreeUI() {
        _isSkillTreeEnabled = !_isSkillTreeEnabled;
        skillTree.gameObject.SetActive(_isSkillTreeEnabled);
        skillTooltip.ShowTooltip(false, null);
    }
}
