using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stat
{
    [SerializeField]
    public int baseValue;

    private List<int> modifiers = new List<int>();

    public int GetValue()
    {
        int outValue = baseValue;

        modifiers.ForEach(delegate (int modVal)
        {
            outValue += modVal;
        });

        return outValue;
    }

    public void AddModifier(int modifier)
    {
        if(modifier != 0)
            modifiers.Add(modifier);
    }

    public void RemoveModifier(int modifier)
    {
        if(modifier != 0)
            modifiers.Remove(modifier);
    }
}
