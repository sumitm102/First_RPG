using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stat
{
    [SerializeField] private int _baseValue;

    public List<int> modifierList;

    public int GetValue() {
        int finalValue = _baseValue;

        foreach (int modifier in modifierList)
            finalValue += modifier;

        return finalValue;
    }

    public void SetDefaultValue(int _value) {
        _baseValue = _value;
    }

    public void AddModifier(int _modifier) => modifierList.Add(_modifier);
    public void RemoveModifier(int _modifier) => modifierList.RemoveAt(_modifier);
    
}
