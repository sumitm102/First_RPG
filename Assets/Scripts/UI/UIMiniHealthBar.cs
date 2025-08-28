using UnityEngine;

public class UIMiniHealthBar : MonoBehaviour
{
    private Entity _entity;

    private void Awake() {
        _entity = GetComponentInParent<Entity>();
    }

    private void OnEnable() {
        _entity.onFlipped += HandleBarFlip;
    }

    private void OnDisable() {
        _entity.onFlipped -= HandleBarFlip;
    }

    private void HandleBarFlip()
    {
        transform.rotation = Quaternion.identity;
    }
}
