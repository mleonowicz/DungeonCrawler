using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    public EnemyData EnemyData;

    public int CurrentHP;

    public void Start()
    {
        CurrentHP = EnemyData.Hp;
        transform.name = EnemyData.Name;
    }

    public void MakeTurn(Player myPlayer)
    {
        EnemyData.Brain.Think(this, myPlayer);    
    }
}
