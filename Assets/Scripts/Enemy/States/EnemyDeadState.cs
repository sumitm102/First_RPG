using UnityEngine;

public class EnemyDeadState : EnemyState {
    public EnemyDeadState(StateMachine sm, int abn, Enemy e) : base(sm, abn, e) {
    }

    public override void EnterState() {
        stateMachine.PreventChangingToNewState();

        anim.enabled = false;
        if (enemy.TryGetComponent<Collider2D>(out var collider))
            collider.enabled = false;

        rb.gravityScale = 12f;
        rb.linearVelocity = new Vector2(rb.linearVelocityX, 15f);


    }
    public override void UpdateState() {

    }

    public override void ExitState() {

    }

}
