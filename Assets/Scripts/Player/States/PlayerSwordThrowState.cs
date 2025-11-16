using UnityEngine;

public class PlayerSwordThrowState : PlayerState {

    private static readonly int _swordThrowPerformedHash = Animator.StringToHash("SwordThrowPerformed");
    private Camera _mainCamera;

    public PlayerSwordThrowState(StateMachine sm, int abn, Player p) : base(sm, abn, p) {
    }

    public override void EnterState() {
        base.EnterState();

        if(_mainCamera != Camera.main)
            _mainCamera = Camera.main;
    }
    public override void UpdateState() {
        base.UpdateState();

        Vector2 directionToMouse = DirectionToMouse();

        player.SetVelocity(0, rb.linearVelocityY);
        player.HandleFlip(directionToMouse.x);


        // TODO: Change the input to something else later
        if (inputSet.Player.CounterAttack.WasPressedThisFrame()) {
            anim.SetBool(_swordThrowPerformedHash, true);

        }

        if(inputSet.Player.AimSword.WasReleasedThisFrame() || triggerCalled)
            stateMachine.ChangeState(player.IdleState);
    }

    public override void ExitState() {
        base.ExitState();
        anim.SetBool(_swordThrowPerformedHash, false);
    }

    public override void UpdateAnimationParameter() {
        base.UpdateAnimationParameter();
    }

    private Vector2 DirectionToMouse() {
        Vector2 playerPosition = player.transform.position;
        Vector2 worldMousePosition = _mainCamera.ScreenToWorldPoint(player.MousePosition);
        
        Vector2 direction = worldMousePosition - playerPosition;
        return direction.normalized;
    }

}
