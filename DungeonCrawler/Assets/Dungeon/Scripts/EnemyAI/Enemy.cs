using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    public EnemyData EnemyData;
    public EnemyStats EnemyStats;

    public void Start()
    {
        transform.name = EnemyData.Name;
    }

    public void MakeTurn(Player myPlayer)
    {
        EnemyData.Brain.Think(this, myPlayer);    
    }
}
