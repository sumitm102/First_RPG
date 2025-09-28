using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Stat
{
    [SerializeField] private float _baseValue;
    [SerializeField] private List<StatModifier> _modifiers = new List<StatModifier>();

    private bool _needsRecalculation = true;
    private float _finalValue;

    public float GetValue() {

        if(_needsRecalculation) {
            _finalValue = GetFinalValue();
            _needsRecalculation = false;
        }

        return _finalValue;
    }

    public void AddModifier(float value, string source) {
        StatModifier modifierToAdd = new StatModifier(value, source);
        _modifiers.Add(modifierToAdd);
        _needsRecalculation = true;
    }

    public void RemoveModifier(string source) {

        // Remove all modifiers with the same source name
        _modifiers.RemoveAll(modifier => source.Equals(modifier.source));
        _needsRecalculation = true;
    }

    private float GetFinalValue() {

        float finalValue = _baseValue;

        foreach (StatModifier modifier in _modifiers) {
            finalValue += modifier.value;
        }

        return finalValue;
    }

    public void SetBaseValue(float value) => _baseValue = value;
}

[Serializable]
public class StatModifier 
{
    public float value;
    public string source;

    public StatModifier(float val, string src) {
        value = val;
        source = src;
    }
}

