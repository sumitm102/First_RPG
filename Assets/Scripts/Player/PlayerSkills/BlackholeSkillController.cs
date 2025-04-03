using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class BlackholeSkillController : MonoBehaviour
{
    [SerializeField] private GameObject _hotkeyPrefab;
    [SerializeField] private List<KeyCode> _keyCodeList;

    public float _maxSize;
    public float _growSpeed;
    public float _shrinkSpeed;
    public bool _canGrow;
    public bool _canShrink;

    private bool _canCreateHotkeys = true;
    private bool _cloneAttackReleased;
    public int attackAmount = 4;
    public float cloneAttackCooldown = 0.3f;
    private float _cloneAttackTimer;

    private List<Transform> _targetList = new List<Transform>();
    private List<GameObject> _hotkeyList = new List<GameObject>();

    private void Update() {

        _cloneAttackTimer -= Time.time;

        if (Input.GetKeyDown(KeyCode.R)) {
            _cloneAttackReleased = true;
            _canCreateHotkeys = false;
            DestroyHotKeys();
        }

        if(_cloneAttackTimer < 0 && _cloneAttackReleased) {
            _cloneAttackTimer = cloneAttackCooldown;

            float _xOffset;

            //Random offset to position the clones either to the right or to left of the enemy
            if (Random.Range(0, 100) > 50)
                _xOffset = 2f;
            else
                _xOffset = -2f;

            int randomIndex = Random.Range(0, _targetList.Count);
            SkillManager.Instance.cloneSkill.CreateClone(_targetList[randomIndex], new Vector3(_xOffset, 0));

            attackAmount--;

            if (attackAmount <= 0) {
                _canShrink = true;
                _cloneAttackReleased = false;
            }
        }

        if (_canGrow && !_canShrink) {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(_maxSize, _maxSize), _growSpeed * Time.deltaTime);
        }

        if (_canShrink) {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(-1f, -1f), _shrinkSpeed * Time.deltaTime);

            if(transform.localScale.x <=  0) 
                Destroy(this.gameObject);
            
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

        if (!_canCreateHotkeys)
            return;

        GameObject newHotkey = Instantiate(_hotkeyPrefab, enemy.transform.position + new Vector3(0, 2), Quaternion.identity);
        _hotkeyList.Add(newHotkey); //Adding the instantiated hot keys to the list to destroy later

        KeyCode chosenKey = _keyCodeList[Random.Range(0, _keyCodeList.Count)];
        _keyCodeList.Remove(chosenKey);


        if (newHotkey.TryGetComponent<BlackholeHotkeyController>(out BlackholeHotkeyController blackholeHotkeyController))
            blackholeHotkeyController.SetupHotkey(chosenKey, enemy.transform, this);
    }

    private void DestroyHotKeys() {
        if (_hotkeyList.Count <= 0)
            return;

        for(int i = 0; i < _hotkeyList.Count; i++)
            Destroy(_hotkeyList[i]);
        
    }

    public void AddEnemyToList(Transform _enemyTransform) => _targetList.Add(_enemyTransform);
    
}
