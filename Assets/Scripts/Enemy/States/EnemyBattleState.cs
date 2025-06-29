using UnityEngine;

public class EnemyBattleState : EnemyState {

    private Transform _playerTransform;

    public EnemyBattleState(StateMachine sm, int abn, Enemy e) : base(sm, abn, e) {
    }

    public override void EnterState() {
        base.EnterState();

        if(_playerTransform == null)
            _playerTransform = enemy.PlayerDetection().transform;
    }
    public override void UpdateState() {
        base.UpdateState();

        if (WithinAttackRange())
            stateMachine.ChangeState(enemy.AttackState);
        else
            enemy.SetVelocity(enemy.BattleMoveSpeed * DirectionToPlayer(), rb.linearVelocityY); 
    }

    public override void ExitState() {
        base.ExitState();
    }

    private float DistanceToPlayer() {

        if (_playerTransform == null)
            return float.MaxValue;

        return Mathf.Abs(_playerTransform.position.x - enemy.transform.position.x);
    }

    private bool WithinAttackRange() =>  DistanceToPlayer() < enemy.AttackDistance;

    private int DirectionToPlayer() {
        if (_playerTransform == null)
            return 0;

        return _playerTransform.position.x > enemy.transform.position.x ? 1 : -1;
    }
    

}
