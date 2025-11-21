using UnityEngine;

public class SkillObjectSword : SkillObjectBase
{
    protected SkillSwordThrow skillSwordThrow;
    protected Rigidbody2D rb;

    protected Transform playerTransform;
    protected bool shouldReturn;
    protected float returnSpeed = 20f;
    protected float maxAllowedDistance = 25f;

    // This is the animation hash used for spin and bounce sword animation
    protected static readonly int spinHash = Animator.StringToHash("Spin");


    protected virtual void Update() {

        // This will make the sword point towards the direction it's flying
        transform.right = rb.linearVelocity;

        HandleReturn();
    }

    public void GetSwordBackToPlayer() {
        shouldReturn = true;

        // This is to make sure the sword can successfully return to the player if the parent transform is moving
        transform.parent = null;
    }

    public virtual void SetupSword(SkillSwordThrow skillSwordThrow, Vector2 direction) {
        rb = GetComponent<Rigidbody2D>();

        // Moves the sword towards the pointed direction
        rb.linearVelocity = direction;

        this.skillSwordThrow = skillSwordThrow;

        playerTransform = this.skillSwordThrow.Player.transform;
        playerStats = this.skillSwordThrow.Player.Stats;
        entityVFX = this.skillSwordThrow.Player.VFX;
        damageScaleData = this.skillSwordThrow.DamageScaleData;

    }

    protected void HandleReturn() {
        float distance = Vector2.Distance(transform.position, playerTransform.position);

        if (distance > maxAllowedDistance)
            GetSwordBackToPlayer();

        if (!shouldReturn)
            return;

        transform.position = Vector2.MoveTowards(transform.position, playerTransform.position, returnSpeed * Time.deltaTime);

        if (distance < .5f)
            Destroy(this.gameObject);
        
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision) {
        StopSword(collision);
        DamageEnemiesInRadius(targetCheck, checkRadius);
    }

    protected void StopSword(Collider2D collision) {
        rb.simulated = false;
        transform.parent = collision.transform;
    }
}
