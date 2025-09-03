using System.Collections;
using UnityEngine;

public class EntityVFX : MonoBehaviour
{

    [Header("On Taking Damage VFX")]
    [SerializeField] private SpriteRenderer _sr;
    [SerializeField] private Material _onDamageVFXMat;
    [SerializeField] private float _onDamageVFXDuration = 0.2f;
    private Material _originalMat;
    private Coroutine _onDamageVFXCoroutine;

    [Header("On Performing Damage VFX")]
    [SerializeField] private Color _hitVFXColor = Color.white;
    [SerializeField] private GameObject _hitVFX;
    [SerializeField] private GameObject _critHitVFX;

    private Entity _entity;

    private void Awake() {
        if(_sr == null)
            _sr = GetComponentInChildren<SpriteRenderer>();

        if(_entity == null)
            _entity = GetComponent<Entity>();

        _originalMat = _sr.material;
    }

    public void CreateOnHitVFX(Transform target, bool isCritDamage) {
        GameObject vfxPrefab = isCritDamage ? _critHitVFX : _hitVFX;
        GameObject vfx = Instantiate(vfxPrefab, target.position, Quaternion.identity);

        SpriteRenderer spriteRenderer = vfx.GetComponentInChildren<SpriteRenderer>();
        if(spriteRenderer != null && !isCritDamage)
            spriteRenderer.color = _hitVFXColor;

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
        _sr.material = _onDamageVFXMat;

        yield return new WaitForSeconds(_onDamageVFXDuration);

        _sr.material = _originalMat;
    }
}
