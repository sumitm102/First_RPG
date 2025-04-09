using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class BlackholeSkillController : MonoBehaviour
{
    [SerializeField] private GameObject _hotkeyPrefab;
    [SerializeField] private List<KeyCode> _keyCodeList;

    private float _maxSize;
    private float _growSpeed;
    private float _shrinkSpeed;
    private bool _canGrow = true;
    private bool _canShrink;

    private bool _canCreateHotkeys = true;
    private bool _cloneAttackReleased;
    private int _attackAmount;
    private float _cloneAttackCooldown;
    private float _cloneAttackTimer;

    private List<Transform> _targetList = new List<Transform>();
    private List<GameObject> _hotkeyList = new List<GameObject>();

    private void Update() {

        _cloneAttackTimer -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.R)) {
            ReleaseCloneAttack();
        }

        CloneAttackLogic();

        if (_canGrow && !_canShrink) 
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(_maxSize, _maxSize), _growSpeed * Time.deltaTime);
        

        if (_canShrink) {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(-1f, -1f), _shrinkSpeed * Time.deltaTime);

            //Destroy when the size of blackhole is less than or equal to 0
            if (transform.localScale.x <= 0)
                Destroy(this.gameObject);

        }
    }

    public void SetupBlackhole(float _maxSize, float _growSpeed, float _shrinkSpeed, int _amountOfAttacks, float _cloneAttackCooldown) {
        this._maxSize = _maxSize;
        this._growSpeed = _growSpeed;
        this._shrinkSpeed = _shrinkSpeed;
        _attackAmount = _amountOfAttacks;
        this._cloneAttackCooldown = _cloneAttackCooldown;

    }

    private void ReleaseCloneAttack() {
        //Making the player invisible before releasing the attack
        PlayerManager.Instance.player.MakeTransparent(true); 

        _cloneAttackReleased = true;
        _canCreateHotkeys = false;
        DestroyHotKeys();
    }

    private void CloneAttackLogic() {
        if (_cloneAttackTimer < 0 && _cloneAttackReleased) {
            _cloneAttackTimer = _cloneAttackCooldown;

            float _xOffset;

            //Set offset randomly to position the clones either to the right or to left of the enemy
            if (Random.Range(0, 100) > 50)
                _xOffset = 2f;
            else
                _xOffset = -2f;

            int randomIndex = Random.Range(0, _targetList.Count);
            SkillManager.Instance.cloneSkill.CreateClone(_targetList[randomIndex], new Vector3(_xOffset, 0));

            _attackAmount--;

            if (_attackAmount <= 0) 
                Invoke("FinishBlackholeAbility" , 1f); // Invokes the function after a second of delay
            
        }
    }

    private void FinishBlackholeAbility() {
        _canShrink = true;
        _cloneAttackReleased = false;
        PlayerManager.Instance.player.ExitBlackholeAbility();
    }

    private void OnTriggerEnter2D(Collider2D _collider) {
        
        if(_collider.TryGetComponent<Enemy>(out Enemy enemy)) {
            enemy.FreezeTime(true);
            CreateHotkey(enemy);

        }
    }

    private void OnTriggerExit2D(Collider2D _collider) {
        if(_collider.TryGetComponent<Enemy>(out Enemy enemy)) {
            enemy.FreezeTime(false);
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
