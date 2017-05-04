using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

[Serializable]
public enum StatType
{
    Armor, Damage
}

[Serializable]
public struct Stat
{
    public StatType stat;
    public int value;

    public override string ToString()
    {
        return "     " + stat + ":" + value + "\n";
    }
}
