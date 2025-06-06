using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    private GameObject _cam;

    [SerializeField] private float _parallaxEffect;
    [SerializeField] private SpriteRenderer _spriteToCheck;

    private float _xPosition;
    private float _length;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _cam = GameObject.FindWithTag("MainCamera");

        //Setting the initial x pos of the background to the variable
        _xPosition = transform.position.x;
        _length = _spriteToCheck.bounds.size.x;
    }

    // Update is called once per frame
    void Update()
    {
        float distanceMoved = _cam.transform.position.x * (1 - _parallaxEffect);
        float distanceToMove = _cam.transform.position.x * _parallaxEffect;

        transform.position = new Vector3(_xPosition + distanceToMove, transform.position.y);

        if (distanceMoved > _xPosition + _length)
            _xPosition += _length;
        else if(distanceMoved < _xPosition - _length)
            _xPosition -= _length;

    }
}
