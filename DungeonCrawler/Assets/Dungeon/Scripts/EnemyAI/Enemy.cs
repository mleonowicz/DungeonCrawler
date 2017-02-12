using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    public EnemyData EnemyData;

    public int CurrentHP;

    public void Start()
    {
        CurrentHP = EnemyData.MaxHp;
        this.transform.name = EnemyData.Name;
        //EnemyData.Brain.SetCallback(CheckGround);
    }

    public void MakeTurn(Player myPlayer)
    {
        EnemyData.Brain.Think(this, myPlayer);    
    }

    
}
