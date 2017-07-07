using UnityEngine;
using System.Collections;

[CreateAssetMenu]
public class PlayerData : ScriptableObject
{
    public string Name;
    public int StartHP;
    public int StartMP;
    public int StartDamage;
    public int StartAttackSpeed;
    public int StartArmor;
}

