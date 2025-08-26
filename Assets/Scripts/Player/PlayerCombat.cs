using UnityEngine;

public class PlayerCombat : EntityCombat
{

    [field: Header("Counter attack details")]
    [field: SerializeField] public float CounterRecovery { get; private set; } = 0.1f;

    private Collider2D[] _targetCollidersForCounterAttack;


    public bool CounterAttackPerformed() {

        bool hasPerformedCountered = false;

        _targetCollidersForCounterAttack = GetDetectedColliders();

        foreach(var target in _targetCollidersForCounterAttack) {
            if (target.TryGetComponent<ICounterable>(out ICounterable counterable)) {
                if (counterable.CanBeCountered) {
                    counterable.HandleCounter();
                    hasPerformedCountered = true;
                }
            }
        }

        return hasPerformedCountered;

    }


}
