using System.Collections.Generic;
using UnityEngine;

public class SkillObjectSwordBounce : SkillObjectSword
{
    [SerializeField] private float _bounceRadius = 10f;
    [SerializeField] private float _minDamageDistance = 0.75f;
    private float _bouceSpeed;
    private int _bounceCount;

    private Collider2D[] _targetColliders;
    private Transform _nextTarget;
    private List<Transform> _selectedTargets = new List<Transform>();


    public override void SetupSword(SkillSwordThrow skillSwordThrow, Vector2 direction) {
        base.SetupSword(skillSwordThrow, direction);

        if (anim != null)
            anim.SetTrigger(spinHash);

        _bouceSpeed = skillSwordThrow.bounceSpeed;
        _bounceCount = skillSwordThrow.bounceCount;
    }


    protected override void Update() {
        HandleReturn();
        HandleBounce();
    }

    private void HandleBounce() {
        if (_nextTarget == null)
            return;

        transform.position = Vector2.MoveTowards(transform.position, _nextTarget.position, _bouceSpeed * Time.deltaTime);

        if(Vector2.Distance(transform.position, _nextTarget.position) < _minDamageDistance) {
            DamageEnemiesInRadius(targetCheck, checkRadius);
            BounceToNextTarget();

            // This ensures that if the sword runs out of bounces or if there is one enemy it can return
            if(_bounceCount <= 0 || _nextTarget == null) {
                _nextTarget = null;
                GetSwordBackToPlayer();
            }
        }
    }

    private void BounceToNextTarget() {
        _nextTarget = GetNextTarget();
        _bounceCount--;
    }

    protected override void OnTriggerEnter2D(Collider2D collision) {
        if (_targetColliders == null) {
            _targetColliders = GetEnemiesAround(targetCheck, _bounceRadius);

            // Along with physics this will also disable collision functionalities
            rb.simulated = false;
        }

        DamageEnemiesInRadius(targetCheck, checkRadius);

        if (_targetColliders.Length <= 1 || _bounceCount <= 0)
            GetSwordBackToPlayer();
        else
            // Doing this instead of calling the BounceToNextTarget method since we don't count the first hit as a bounce
            _nextTarget = GetNextTarget();
    }


    private Transform GetNextTarget() {
        List<Transform> validTargets = GetValidTargets();

        int randomIndex = Random.Range(0, validTargets.Count);

        Transform nextTarget = validTargets[randomIndex];
        _selectedTargets.Add(nextTarget);

        return nextTarget;
    }

    private List<Transform> GetValidTargets() {
        List<Transform> validTargets = new List<Transform>();
        List<Transform> aliveTargets = GetAliveTargets();

        foreach(var target in aliveTargets)
            if(target != null && !_selectedTargets.Contains(target.transform))
                validTargets.Add(target.transform);

        if (validTargets.Count > 0)
            return validTargets;

        // If all targets have been selected before, clear the list for it and return the alive targets
        _selectedTargets.Clear();
        return aliveTargets;
    }

    private List<Transform> GetAliveTargets() {
        List<Transform> aliveTargets = new List<Transform>();

        foreach(var target in _targetColliders) 
            if(target != null)
                aliveTargets.Add(target.transform);
        
        return aliveTargets;
    }
}
