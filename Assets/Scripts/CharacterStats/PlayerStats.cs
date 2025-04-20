using UnityEngine;

public class PlayerStats : CharacterStats
{
    private Player _player;


    protected override void Start() {
        base.Start();

        _player = GetComponent<Player>();
    }

    public override void TakeDamage(int _damage) {
        base.TakeDamage(_damage);

        _player.DamageEffect();
    }
}
