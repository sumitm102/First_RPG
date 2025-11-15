
public class ElementalEffectData
{
    public float chillDuration;
    public float chillSlowMultiplier;

    public float burnDuration;
    public float burnDamage;

    public float electrifyDuration;
    public float electrifyDamage;
    public float electrifyCharge;

    public ElementalEffectData(EntityStats entityStas, DamageScaleData damageScaleData) {
        chillDuration = damageScaleData.chillDuration;
        chillSlowMultiplier = damageScaleData.chillSlowMultiplier;

        burnDuration = damageScaleData.burnDuration;
        burnDamage = entityStas.offenseStats.fireDamage.GetValue();

        electrifyDuration = damageScaleData.electrifyDuration;
        electrifyDamage = entityStas.offenseStats.lightningDamage.GetValue();
        electrifyCharge = damageScaleData.electrifyCharge;
    }
}
