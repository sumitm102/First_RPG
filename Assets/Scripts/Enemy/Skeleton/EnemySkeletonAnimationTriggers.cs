using UnityEngine;

public class EnemySkeletonAnimationTriggers : MonoBehaviour
{
    private EnemySkeleton _enemy => GetComponentInParent<EnemySkeleton>();

    public void AnimationTrigger() {
        _enemy.AnimationFinishTrigger();
    }
}
