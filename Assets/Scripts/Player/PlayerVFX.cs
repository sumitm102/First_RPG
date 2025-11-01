using System.Collections;
using UnityEngine;

public class PlayerVFX : EntityVFX
{
    [Header("Image Echo VFX")]
    [Range(0.01f, 0.2f)]
    [SerializeField] private float _imageEchoInterval = 0.05f;
    [SerializeField] private GameObject _imageEchoPrefab;
    [SerializeField] private bool _imageEchoEnabled = true;

    private Coroutine _imageEchoCo;

    public void ImageEchoEffect(float duration) {
        if (!_imageEchoEnabled)
            return;

        if( _imageEchoCo != null ) {
            StopCoroutine(_imageEchoCo);
        }

        _imageEchoCo = StartCoroutine(ImageEchoEffectCo(duration));
    }

    private IEnumerator ImageEchoEffectCo(float duration) {
        float timeTracker = 0f;

        while(timeTracker < duration) {
            CreateImageEcho();

            yield return new WaitForSeconds(_imageEchoInterval);

            timeTracker += _imageEchoInterval;
        }
    }

    private void CreateImageEcho() {
        GameObject imageEcho = Instantiate(_imageEchoPrefab, transform.position, transform.rotation);

        imageEcho.GetComponentInChildren<SpriteRenderer>().sprite = sr.sprite;
    }
}
