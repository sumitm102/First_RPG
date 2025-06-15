using UnityEngine;

public class PlayerAnimationTriggers : MonoBehaviour
{

    private Player _player;

    private void Awake() {
        _player = GetComponentInParent<Player>();
    }

    private void CurrentStateTrigger() {
        _player.CallAnimationTrigger();
    }
}
