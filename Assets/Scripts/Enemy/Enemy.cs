using System.Collections;
using UnityEngine;

public class Enemy : Entity
{
    [field: Header("Movement details")]
    [field: SerializeField] public float MoveSpeed { get; private set; } = 1.4f;
    [field: SerializeField] public float IdleTime { get; private set; } = 2f;
    [field: SerializeField, Range(0, 2)] public float MoveAnimSpeedMultiplier { get; private set; } = 1f;

    [field: Header("Battle details")]
    [field: SerializeField] public float BattleMoveSpeed { get; private set; } = 3f;
    [field: SerializeField] public float AttackDistance { get; private set; } = 2f;

    [field:Header("Player detection")]
    [field:SerializeField] public Transform PlayerCheck { get; private set; } 
    [field:SerializeField] public LayerMask PlayerDetectionLayer { get; private set;}
    [field: SerializeField] public float PlayerCheckDistance { get; private set; } = 10f;
    

    #region States

    public EnemyIdleState IdleState;
    public EnemyMoveState MoveState;
    public EnemyAttackState AttackState;
    public EnemyBattleState BattleState;

    #endregion


    protected override void Start() {
        base.Start();

        StateMachine.Initialize(IdleState);
    }

    public RaycastHit2D PlayerDetection() {
        RaycastHit2D hit =  Physics2D.Raycast(PlayerCheck.position, Vector3.right * FacingDir, PlayerCheckDistance, groundDetectionLayer | PlayerDetectionLayer);

        if (hit.collider == null || hit.collider.gameObject.layer != LayerMask.NameToLayer("Player"))
            return default;
        

        return hit;
    }

    protected override void OnDrawGizmos() {
        base.OnDrawGizmos();

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(PlayerCheck.position, new Vector3(PlayerCheck.position.x + (FacingDir * PlayerCheckDistance), PlayerCheck.position.y));

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(PlayerCheck.position, new Vector3(PlayerCheck.position.x + (FacingDir * AttackDistance), PlayerCheck.position.y));
    }


}
