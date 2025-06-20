using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    private Camera _mainCamera;
    private float _lastHorizontalCameraPos;
    private float _cameraHalfWidth;

    [SerializeField] private ParallaxLayer[] _backgroundLayers;

    private void Awake() {
        _mainCamera = Camera.main;
        _cameraHalfWidth = _mainCamera.orthographicSize * _mainCamera.aspect;
        InitializeLayerWidth();
    }

    // Using fixed update to avoid jittering when background is static
    // Cinemachine brain's update mode was set to fixed update
    // Interpolate in player rb was set to none
    private void FixedUpdate() {
        float currentHorizontalCameraPos = _mainCamera.transform.position.x;
        float distanceToMove = currentHorizontalCameraPos - _lastHorizontalCameraPos;
        _lastHorizontalCameraPos = currentHorizontalCameraPos;

        float cameraLeftEdge = currentHorizontalCameraPos - _cameraHalfWidth;
        float cameraRightEdge = currentHorizontalCameraPos + _cameraHalfWidth;

        foreach(var layer in _backgroundLayers) {
            layer.Move(distanceToMove);
            layer.LoopBackground(cameraLeftEdge, cameraRightEdge);
        }
    }

    private void InitializeLayerWidth() {
        foreach (var layer in _backgroundLayers) {
            layer.CalculateImageWidth();
        }
    }
}
