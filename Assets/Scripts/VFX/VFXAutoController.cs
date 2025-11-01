using System.Collections;
using UnityEngine;

public class VFXAutoController : MonoBehaviour
{
    [SerializeField] private bool _autoDestroy = true;
    [SerializeField] private float _destroyDelay = 1f;
    [Space]
    [SerializeField] private bool _enableRandomOffset = true;
    [SerializeField] private bool _enableRandomRotation = true;

    [Header("Random position")]
    [SerializeField] private float _xMinOffset = -0.3f;
    [SerializeField] private float _xMaxOffset = 0.3f;
    [Space]
    [SerializeField] private float _yMinOffset = -0.3f;
    [SerializeField] private float _yMaxOffset = 0.3f;

    [Header("Random rotation")]
    [SerializeField] private float _minRotation = 0f;
    [SerializeField] private float _maxRotation = 360f;

    [Header("Fade effect")]
    [SerializeField] private bool _canFade;
    [SerializeField] private float _fadeSpeed = 1f;


    private SpriteRenderer _sr;


    private void Awake() {
        _sr = GetComponentInChildren<SpriteRenderer>();
    }

    private void Start() {

        if (_canFade)
            StartCoroutine(FadeCo());

        ApplyRandomOffset();
        ApplyRandomRotation();

        if(_autoDestroy)
            Destroy(gameObject, _destroyDelay);
    }

    private IEnumerator FadeCo() {
        Color targetColor = Color.white;

        while(targetColor.a > 0) {
            targetColor.a -= (_fadeSpeed * Time.deltaTime);
            _sr.color = targetColor;

            yield return null;
        }

        _sr.color = targetColor;
    }

    private void ApplyRandomOffset() {
        if (!_enableRandomOffset)
            return;

        float xOffset = Random.Range(_xMinOffset, _xMaxOffset);
        float yOffset = Random.Range(_yMinOffset, _yMaxOffset);

        transform.position += new Vector3(xOffset, yOffset);
    }

    private void ApplyRandomRotation() {
        if (!_enableRandomRotation)
            return;

        float zRotation = Random.Range(_minRotation, _maxRotation);
        transform.Rotate(0, 0, zRotation);
    }
}
