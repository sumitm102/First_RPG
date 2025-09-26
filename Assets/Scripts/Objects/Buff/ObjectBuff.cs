using System;
using System.Collections;
using UnityEngine;


[Serializable]
public class Buff {
    public E_StatType type;
    public float value;
}


public class ObjectBuff : MonoBehaviour
{
    [Header("Floaty movement details")]
    [SerializeField] private float _floatingSpeed = 1f;
    [SerializeField] private float _floatingRange = 0.1f;
    private Vector3 _startingPosition;

    [Header("Buff details")]
    [SerializeField] private Buff[] _buffs;
    [SerializeField] private float _buffDuration = 4f;
    [SerializeField] private bool _canBeUsed = true;
    [SerializeField] private string _buffName;


    private SpriteRenderer _spriteRenderer;
    private EntityStats _playerStats;

    private void Awake() {

        _startingPosition = transform.position;
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void Update() {

        
        float yOffset = Mathf.Sin(_floatingSpeed * Time.time) * _floatingRange;
        transform.position = _startingPosition + new Vector3(0, yOffset, 0);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (!_canBeUsed)
            return;

        _playerStats = collision.GetComponent<EntityStats>();

        StartCoroutine(BuffCo(_buffDuration));

    }

    private IEnumerator BuffCo(float duration) {
        _canBeUsed = false;

        // This is to make the object disappear since it can't be destroyed or turned off right here due to interrupting the coroutine
        _spriteRenderer.color = Color.clear;

        ApplyBuff(true);

        yield return new WaitForSeconds(duration);

        // Removes all the buffs
        ApplyBuff(false);

        Destroy(this.gameObject);


    }

    private void ApplyBuff(bool applyBuff) {

        // Looping in case the object can buff multiple different stats
        foreach (var buff in _buffs) {
            Stat playerStatToBuff = _playerStats.GetStatByType(buff.type);

            if(applyBuff)
                playerStatToBuff.AddModifier(buff.value, _buffName);
            else
                playerStatToBuff.RemoveModifier(_buffName);
        }
    }
}
