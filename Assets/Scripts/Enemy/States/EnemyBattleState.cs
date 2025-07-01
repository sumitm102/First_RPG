using UnityEngine;

public class EnemyBattleState : EnemyState {

    private Transform _playerTransform;
    private float _lastTimeInBattle;

    public EnemyBattleState(StateMachine sm, int abn, Enemy e) : base(sm, abn, e) {
    }

    public override void EnterState() {
        base.EnterState();

        _lastTimeInBattle = Time.time;

        if (_playerTransform == null)
            _playerTransform = enemy.GetPlayerReference();

        // This makes sure enemy backs up if the player character is too close
        if (ShouldRetreat()) {
            rb.linearVelocity = new Vector2(enemy.RetreatVelocity.x * -1f * DirectionToPlayer(), enemy.RetreatVelocity.y); // Not using set velocity method since it'll flip the character when negative velocity is applied
            enemy.HandleFlip(DirectionToPlayer());
        }
    }

    public override void UpdateState() {
        base.UpdateState();

        if (enemy.PlayerDetected())
            _lastTimeInBattle = Time.time;

        // Enemy is going to stay to battle mode for a certain duration even after the player stops getting detected
        // This makes sure that after the duration has been passed it goes back to idle
        if (IsBattleTimeOver()) {
            _playerTransform = null;
            stateMachine.ChangeState(enemy.IdleState);
        }

        if (WithinAttackRange() && enemy.PlayerDetected())
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

    private bool IsBattleTimeOver() => Time.time - _lastTimeInBattle > enemy.BattleTimeDuration;
    private bool ShouldRetreat() => DistanceToPlayer() < enemy.MinRetreatDistance;
    
    

}
