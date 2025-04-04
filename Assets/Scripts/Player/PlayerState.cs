
using Unity.VisualScripting;
using UnityEngine;

public class PlayerState
{
    protected Player player;
    protected PlayerStateMachine playerStateMachine;
    private string _animBoolName;

    protected float xInput;
    protected float yInput;
    protected float stateTimer;
    protected bool triggerCalled;

    public PlayerState(Player _player, PlayerStateMachine _playerStateMachine, string _animBoolName) {
        this.player = _player;
        this.playerStateMachine = _playerStateMachine;
        this._animBoolName = _animBoolName;
    }

    public virtual void EnterState() {
        player.animator.SetBool(_animBoolName, true);
        triggerCalled = false;
    }
    public virtual void UpdateState() {

        //Decreasing the timer and making it accessible to other state classes
        stateTimer -= Time.deltaTime;

        //Using raw input to avoid smoothing
        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");

        //Changing the yVelocity parameter inside the jump blend tree based on rigidbody's vertical velocity
        player.animator.SetFloat("yVelocity", player.rbody.linearVelocityY);
    }
    public virtual void ExitState() {
        player.animator.SetBool(_animBoolName, false);
    }

    public virtual void AnimationFinishTrigger() {
        triggerCalled = true;
    }

}
