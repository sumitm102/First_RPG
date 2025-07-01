using System.Collections;
using UnityEngine;

public class EntityVFX : MonoBehaviour
{

    [Header("On Damage VFX")]
    [SerializeField] private SpriteRenderer _sr;
    [SerializeField] private Material _onDamageVFXMat;
    [SerializeField] private float _onDamageVFXDuration = 0.2f;
    private Material _originalMat;
    private Coroutine _onDamageVFXCoroutine;

    private void Awake() {
        if(_sr == null)
            _sr = GetComponentInChildren<SpriteRenderer>();

        _originalMat = _sr.material;
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
