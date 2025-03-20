using UnityEngine;

public class Enemy : Entity
{
    [Header("Move info")]
    public float moveSpeed;
    public float idleTime;

    public EnemyStateMachine enemyStateMachine { get; private set; }



    protected override void Awake() {
        base.Awake();

        enemyStateMachine = new EnemyStateMachine();
    }

    protected override void Start() {
        base.Start();
    }


    protected override void Update() {
        base.Update();

        enemyStateMachine.currentState.UpdateState();
    }
}
