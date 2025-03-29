using UnityEngine;

public class Skill : MonoBehaviour
{
    [SerializeField] protected float cooldown;
    protected float cooldownTimer;

    protected Player player;

    protected virtual void Start() {
        player = PlayerManager.Instance.player;
    }

    protected virtual void Update() {
        cooldownTimer -= Time.deltaTime;
    }

    public virtual bool CanUseSkill() {
        if (cooldownTimer < 0) {
            cooldownTimer = cooldown;
            UseSkill();

            return true;
        }

        return false;
    }

    public virtual void UseSkill() {

    }
}
