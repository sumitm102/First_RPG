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

    private void Start() {

        ApplyRandomOffset();
        ApplyRandomRotation();

        if(_autoDestroy)
            Destroy(gameObject, _destroyDelay);
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
