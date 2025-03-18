
using Unity.VisualScripting;
using UnityEngine;

public class PlayerState
{
    protected Player player;
    protected PlayerStateMachine playerStateMachine;

    private string _animBoolName;
    public PlayerState(Player _player, PlayerStateMachine _playerStateMachine, string _animBoolName) {
        this.player = _player;
        this.playerStateMachine = _playerStateMachine;
        this._animBoolName = _animBoolName;
    }

    public virtual void EnterState() {
        Debug.Log("Entering " + _animBoolName);
    }
    public virtual void UpdateState() {
        Debug.Log("Updating " + _animBoolName);
    }
    public virtual void ExitState() {
        Debug.Log("Exiting " + _animBoolName);
    }



}
