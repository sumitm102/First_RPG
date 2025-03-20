using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Animator enemyAnimator { get; private set; }
    public EnemyStateMachine enemyStateMachine { get; private set; }
    public EnemyIdleState enemyIdleState { get; private set; }


    private void Awake() {
        enemyStateMachine = new EnemyStateMachine();
        enemyIdleState = new EnemyIdleState(this, enemyStateMachine, "Idle");
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemyStateMachine.Initialize(enemyIdleState);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
