using UnityEngine;

public class EntityAnimationTriggers : MonoBehaviour
{

    private Entity _entity;

    private void Awake() {
        _entity = GetComponentInParent<Entity>();
    }

    private void CurrentStateTrigger() {
        _entity.CurrentStateAnimationTrigger();
    }
}
