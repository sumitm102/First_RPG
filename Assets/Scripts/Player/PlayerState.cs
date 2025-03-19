
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

    public PlayerState(Player _player, PlayerStateMachine _playerStateMachine, string _animBoolName) {
        this.player = _player;
        this.playerStateMachine = _playerStateMachine;
        this._animBoolName = _animBoolName;
    }

    public virtual void EnterState() {
        player.playerAnimator.SetBool(_animBoolName, true);
    }
    public virtual void UpdateState() {

        //Decreasing the timer and making it accessible to other state classes
        stateTimer -= Time.deltaTime;

        //Using raw input to avoid smoothing
        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");

        //Changing the yVelocity parameter inside the jump blend tree based on rigidbody's vertical velocity
        player.playerAnimator.SetFloat("yVelocity", player.playerRigidbody.linearVelocityY);
    }
    public virtual void ExitState() {
        player.playerAnimator.SetBool(_animBoolName, false);
    }



}
