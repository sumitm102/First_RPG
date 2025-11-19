using UnityEngine;

public class SkillObjectSwordPierce : SkillObjectSword
{
    private int amountToPierce;

    public override void SetupSword(SkillSwordThrow skilSwordThrow, Vector2 direction) {
        base.SetupSword(skilSwordThrow, direction);

        amountToPierce = skillSwordThrow.amountToPierce;
    }

    protected override void OnTriggerEnter2D(Collider2D collision) {
        bool grounHit = collision.gameObject.layer == LayerMask.NameToLayer("Ground");

        if(amountToPierce <= 0 || grounHit) {
            StopSword(collision);
            DamageEnemiesInRadius(targetCheck, 0.3f);
            return;
        }

        // Using a small radius since multiple enemies within a small range will trigger this method multiple times instead of once
        DamageEnemiesInRadius(targetCheck, 0.3f);
        amountToPierce--;
    }
}
