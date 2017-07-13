using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemyState
{
    void Execute();
    void Enter(EnemyPlatform enemy);
    void Exit();
    void OnCollisionEnter(Collision2D coll);
}