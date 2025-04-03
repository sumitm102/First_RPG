using TMPro;
using UnityEngine;

public class BlackholeHotkeyController : MonoBehaviour
{
    private KeyCode _hotkey;
    private TextMeshProUGUI _textMeshPro;
    private Transform _enemyTransform;
    private BlackholeSkillController _blackholeSkillController;
    private SpriteRenderer _spriteRenderer;

    public void SetupHotkey(KeyCode _newHotkey, Transform _newEnemyTransform, BlackholeSkillController _newBlackholeSkillController) {
        _textMeshPro = GetComponentInChildren<TextMeshProUGUI>();
        _spriteRenderer = GetComponent<SpriteRenderer>();

        _hotkey = _newHotkey;
        _enemyTransform = _newEnemyTransform;
        _blackholeSkillController = _newBlackholeSkillController;

        if(_textMeshPro != null)
            _textMeshPro.text = _newHotkey.ToString();
    }

    private void Update() {
        if (Input.GetKeyDown(_hotkey)) {
            _blackholeSkillController.AddEnemyToList(_enemyTransform);

            _textMeshPro.color = Color.clear;
            _spriteRenderer.color = Color.clear;
        }
    }
}
