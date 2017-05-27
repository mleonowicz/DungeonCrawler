using UnityEngine;
using System.Collections;

[CreateAssetMenu]
public class EnemyData : ScriptableObject
{
    public EnemyBrain Brain;

    public string Name;
    public int Hp;

    public int Damage;
}
