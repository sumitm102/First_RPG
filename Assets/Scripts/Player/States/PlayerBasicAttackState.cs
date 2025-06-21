using UnityEngine;

public class PlayerBasicAttackState : PlayerState {

    private float _attackVelocityTimer;
    private static readonly int _attackIndexHash = Animator.StringToHash("BasicAttackIndex");

    private const int _FirstComboIndex = 1;
    private int _comboIndex = _FirstComboIndex;
    private int _comboLimit = 3;
    private int _attackDir;

    private float _lastTimeAttacked;
    private bool _comboAttackQueued;
    public PlayerBasicAttackState(StateMachine sm, int abn, Player p) : base(sm, abn, p) {


        // Added as a precaution in case to resolve conflict between combo limit and velocity array length
        if(_comboLimit != player.AttackVelocity.Length)
            _comboLimit = player.AttackVelocity.Length;
    }

    public override void EnterState() {
        base.EnterState();

        _comboAttackQueued = false;
        ManageComboIndex();

        // Defining attack direction according to player input or the character's facing direction
        _attackDir = player.MoveInput.x != 0 ? ((int)player.MoveInput.x) : player.FacingDir;

        anim.SetInteger(_attackIndexHash, _comboIndex);

        ApplyAttackVelocity();
    }


    public override void UpdateState() {
        base.UpdateState();
        HandleAttackVelocity();

        if (inputSet.Player.Attack.WasPressedThisFrame())
            QueueNextAttack();
        

        if (triggerCalled) 
            HandleStateExit();
    }


    public override void ExitState() {
        base.ExitState();

        _comboIndex++;
        _lastTimeAttacked = Time.time;
    }

    private void ApplyAttackVelocity() {
        _attackVelocityTimer = player.AttackVelocityDuration;
        Vector2 currentAttackVelocity = player.AttackVelocity[_comboIndex - 1];

        player.SetVelocity(currentAttackVelocity.x * _attackDir, currentAttackVelocity.y);
    }

    private void HandleAttackVelocity() {
        _attackVelocityTimer -= Time.deltaTime;

        if(_attackVelocityTimer < 0)
            player.SetVelocity(0, rb.linearVelocityY);

    }

    private void ManageComboIndex() {

        // Reset to the first combo if attack chain is completed or if the attack window is greater than the current time in game
        if (_comboIndex > _comboLimit || Time.time > _lastTimeAttacked + player.ComboResetTime)
            _comboIndex = _FirstComboIndex;
    }

    private void HandleStateExit() {
        if (_comboAttackQueued) {

            anim.SetBool(animBoolName, false);

            player.EnterAttackStateWithDelay();
        }
        else
            stateMachine.ChangeState(player.IdleState);
    }

    private void QueueNextAttack() {
        if (_comboIndex < _comboLimit)
            _comboAttackQueued = true;
    }
}
