using System.Collections;
using UnityEngine;

public class EntityVFX : MonoBehaviour
{

    [Header("On Taking Damage VFX")]
    [SerializeField] protected SpriteRenderer sr;
    [SerializeField] private Material _onDamageVFXMat;
    [SerializeField] private float _onDamageVFXDuration = 0.2f;
    private Material _originalMat;
    private Coroutine _onDamageVFXCoroutine;

    [Header("On Performing Damage VFX")]
    [SerializeField] private Color _hitVFXColor = Color.white;
    [SerializeField] private GameObject _hitVFX;
    [SerializeField] private GameObject _critHitVFX;

    [Header("Element Colors")]
    [SerializeField] private Color _chillVFXColor = Color.cyan;
    [SerializeField] private Color _burnVFXColor = Color.red;
    [SerializeField] private Color _electrifyVFXColor = Color.yellow;
    private Color _originalHitVFXColor;

    private Entity _entity;

    private void Awake() {
        if(sr == null)
            sr = GetComponentInChildren<SpriteRenderer>();

        if(_entity == null)
            _entity = GetComponent<Entity>();

        _originalMat = sr.material;
        _originalHitVFXColor = _hitVFXColor;
    }

    public void CreateOnHitVFX(Transform target, bool isCritDamage, E_ElementType elementType) {
        GameObject vfxPrefab = isCritDamage ? _critHitVFX : _hitVFX;
        GameObject vfx = Instantiate(vfxPrefab, target.position, Quaternion.identity);

        SpriteRenderer spriteRenderer = vfx.GetComponentInChildren<SpriteRenderer>();
        if(spriteRenderer != null && !isCritDamage)
            spriteRenderer.color = GetElementColor(elementType);

        // crit hit vfx should be facing away from the entity that is performing the hit
        if (_entity.FacingDir == -1 && isCritDamage)
            vfx.transform.Rotate(0, 180, 0);
    }

    public void PlayOnDamageVFX() {

        if (_onDamageVFXCoroutine != null)
            StopCoroutine(_onDamageVFXCoroutine);

        _onDamageVFXCoroutine = StartCoroutine(OnDamageVFXCo());
    }

    private IEnumerator OnDamageVFXCo() {
        sr.material = _onDamageVFXMat;

        yield return new WaitForSeconds(_onDamageVFXDuration);

        sr.material = _originalMat;
    }

    public Color GetElementColor(E_ElementType elementType) {
        switch(elementType) {
            case E_ElementType.Fire:
                return _burnVFXColor;
            case E_ElementType.Ice:
                return _chillVFXColor;
            case E_ElementType.Lightning:
                return _electrifyVFXColor;
            default:
                return Color.white;
        }
    }

    public void PlayOnStatusVFX(float duration, E_ElementType elementType) {
        Color statusEffectColor = Color.white;

        switch (elementType) {
            case E_ElementType.Fire:
                statusEffectColor = _burnVFXColor;
                break;
            case E_ElementType.Ice:
                statusEffectColor = _chillVFXColor;
                break;
            case E_ElementType.Lightning:
                statusEffectColor= _electrifyVFXColor;
                break;
        }

        StartCoroutine(PlayStatusVFXCo(duration, statusEffectColor));
    }

    private IEnumerator PlayStatusVFXCo(float duration, Color statusEffectColor) {
        float tickInterval = 0.25f;
        float timeHasPassed = 0;

        Color lightColor = statusEffectColor * 1.2f;
        Color darkColor = statusEffectColor * 0.8f;

        bool toggle = false;

        while (timeHasPassed < duration) {
            sr.color = toggle ? lightColor : darkColor;
            toggle = !toggle;

            yield return new WaitForSeconds(tickInterval);

            timeHasPassed += tickInterval;
        }

        sr.color = Color.white;
    }

    public void StopAllVFX() {

        // Stops all coroutines on this script
        StopAllCoroutines();

        // In case the coroutine stops while there is an ongoing visual effect
        sr.color = Color.white;
        sr.material = _originalMat;
    }
}
