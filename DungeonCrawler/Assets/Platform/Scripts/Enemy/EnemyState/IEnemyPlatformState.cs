using UnityEngine;
using System.Collections;

public interface IEnemyPlatformState
{
    void Execute();
    void Enter(EnemyPlatform enemy);
    void Exit();
    void OnTriggerEnter(Collider2D other);
}
