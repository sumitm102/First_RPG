using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class BlackholeSkillController : MonoBehaviour
{
    [SerializeField] private GameObject _hotkeyPrefab;
    [SerializeField] private List<KeyCode> _keyCodeList;

    public float _maxSize;
    public float _growSpeed;
    public bool _canGrow;

    private List<Transform> _targetList = new List<Transform>();

    private void Update() {
        if (_canGrow) {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(_maxSize, _maxSize), _growSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D _collider) {
        
        if(_collider.TryGetComponent<Enemy>(out Enemy enemy)) {
            enemy.FreezeTime(true);
            CreateHotkey(enemy);

        }
    }

    private void CreateHotkey(Enemy enemy) {

        if (_keyCodeList.Count <= 0) {
            Debug.LogWarning("KeyCode List doesn't have enough elements");
            return;
        }

        GameObject newHotkey = Instantiate(_hotkeyPrefab, enemy.transform.position + new Vector3(0, 2), Quaternion.identity);

        KeyCode chosenKey = _keyCodeList[Random.Range(0, _keyCodeList.Count)];
        _keyCodeList.Remove(chosenKey);


        if (newHotkey.TryGetComponent<BlackholeHotkeyController>(out BlackholeHotkeyController blackholeHotkeyController))
            blackholeHotkeyController.SetupHotkey(chosenKey, enemy.transform, this);
    }

    public void AddEnemyToList(Transform _enemyTransform) => _targetList.Add(_enemyTransform);
    
}
