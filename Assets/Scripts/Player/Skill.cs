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

    protected virtual Transform FindClosestEnemy(Transform _checkTransform, float _checkRadius) {
        //Checks and stores all colliders within 5m radius of itself
        Collider2D[] colliders = Physics2D.OverlapCircleAll(_checkTransform.position, _checkRadius);

        float closestDistance = Mathf.Infinity;
        Transform closestEnemy = null;

        //Looping through colliders with enemy components while distance checking enemies to get the closest one
        foreach (var collider in colliders) {
            if (collider.TryGetComponent<Enemy>(out Enemy enemy)) {
                float distanceToEnemy = Vector2.Distance(_checkTransform.position, enemy.transform.position);

                if (distanceToEnemy < closestDistance) {
                    closestDistance = distanceToEnemy;
                    closestEnemy = enemy.transform;
                }
            }
        }

        return closestEnemy;
    }
}
