using System.Collections;
using UnityEngine;

public class EntityFX : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;

    [Header("Flash FX")]
    private Material _originalMaterial;
    [SerializeField] private Material _flashFXMaterial;
    [SerializeField] private float _flashDuration = 0.2f;

    private void Start() {
        if(_spriteRenderer == null) _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _originalMaterial = _spriteRenderer.material;
    }

    private IEnumerator FlashFX() {
        _spriteRenderer.material = _flashFXMaterial;
        yield return new WaitForSeconds(_flashDuration);
        _spriteRenderer.material = _originalMaterial;
    }
}
