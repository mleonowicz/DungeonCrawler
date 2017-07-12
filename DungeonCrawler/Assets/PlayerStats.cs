using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PlayerStats : ScriptableObject
{
    public int HP;
    public int MP;

    public int Damage;
    public int Armor;
    public int AttackSpeed;
    public int MovementSpeeed;
}
