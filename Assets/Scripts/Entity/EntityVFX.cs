using System.Collections;
using UnityEngine;

public class EntityVFX : MonoBehaviour
{

    [Header("On Take Damage VFX")]
    [SerializeField] private SpriteRenderer _sr;
    [SerializeField] private Material _onDamageVFXMat;
    [SerializeField] private float _onDamageVFXDuration = 0.2f;
    private Material _originalMat;
    private Coroutine _onDamageVFXCoroutine;

    [Header("On Performing Damage VFX")]
    [SerializeField] private Color _hitVFXColor = Color.white;
    [SerializeField] private GameObject _hitVFX;

    private void Awake() {
        if(_sr == null)
            _sr = GetComponentInChildren<SpriteRenderer>();

        _originalMat = _sr.material;
    }

    public void CreateOnHitVFX(Transform target) {
        GameObject vfx = Instantiate(_hitVFX, target.position, Quaternion.identity);

        SpriteRenderer spriteRenderer = vfx.GetComponentInChildren<SpriteRenderer>();
        if(spriteRenderer != null )
            spriteRenderer.color = _hitVFXColor;
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
