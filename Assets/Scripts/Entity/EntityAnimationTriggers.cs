using UnityEngine;

public class EntityAnimationTriggers : MonoBehaviour
{

    private Entity _entity;
    private EntityCombat _entityCombat;

    protected virtual void Awake() {
        _entity = GetComponentInParent<Entity>();
        _entityCombat = GetComponentInParent<EntityCombat>();
    }

    private void CurrentStateTrigger() {
        if (_entity == null) {
            Debug.Log("Entity is null on " + this.gameObject.name);
            return;
        }

        _entity.CurrentStateAnimationTrigger();
    }

    private void AttackTrigger() {
        if(_entityCombat == null) {
            Debug.Log("Entity Combat is null" + this.gameObject.name);
            return;
        }

        _entityCombat.PerformAttack();
    }
}
