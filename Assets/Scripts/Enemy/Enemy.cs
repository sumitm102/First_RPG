using System.Collections;
using UnityEngine;

public class Enemy : Entity
{
    [field: Header("Movement details")]
    [field: SerializeField] public float MoveSpeed { get; private set; } = 1.4f;
    [field: SerializeField] public float IdleTime { get; private set; } = 2f;
    [field: SerializeField, Range(0, 2)] public float MoveAnimSpeedMultiplier { get; private set; } = 1f;

    #region States

    public EnemyIdleState IdleState;
    public EnemyMoveState MoveState;

    #endregion


    protected override void Start() {
        base.Start();

        StateMachine.Initialize(IdleState);
    }


}
