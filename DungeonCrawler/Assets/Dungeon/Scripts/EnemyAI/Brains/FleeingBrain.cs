using UnityEngine;
using System.Collections;

[CreateAssetMenu]
public class FleeingBrain : EnemyBrain
{

    public override void Think(Enemy enemy, Player myPlayer)
    {
        // var disc = Vector3.Distance(myPlayer.transform.position, enemy.transform.position);
        if (enemy.transform.position == myPlayer.transform.position)
        {
            return;
        }

        if (!(Vector3.Distance(myPlayer.transform.position, enemy.transform.position) < PlayerDetectionRange))
        {
            Movement(enemy.transform);          
        }
        else
        {
            var dir = myPlayer.transform.position - enemy.transform.position;
            dir = dir.normalized;
            /* 
            var cpV = new Vector3(Mathf.Abs(v.x), Mathf.Abs(v.y)); // Made using Sinus <3

            var sin1 = Vector3.Angle(new Vector3(0, 1), cpV);
            var sin2 = Vector3.Angle(new Vector3(1, 0), cpV); */

            // Using LevelGenerator Instanse because this class inheritates from ScriptableObject and it doenst have "Start Courutine" Function

            if (Mathf.Abs(dir.x) >= Mathf.Abs(dir.y))
            {
                dir = new Vector3(Mathf.Sign(dir.x) * -1, 0);
                if (CanMove(enemy.transform.position, dir))
                    LevelGenerator.instance.StartCoroutine(EnemyMoveAnim(dir, enemy.transform));
                else if (CanMove(enemy.transform.position, Vector3.up))
                    LevelGenerator.instance.StartCoroutine(EnemyMoveAnim(Vector3.up, enemy.transform));
                else if (CanMove(enemy.transform.position, Vector3.down))
                    LevelGenerator.instance.StartCoroutine(EnemyMoveAnim(Vector3.down, enemy.transform));
            }
            else
            {
                dir = new Vector3(0, Mathf.Sign(dir.y) * -1);
                if (CanMove(enemy.transform.position, dir))
                    LevelGenerator.instance.StartCoroutine(EnemyMoveAnim(dir, enemy.transform));
                else if (CanMove(enemy.transform.position, Vector3.left))
                    LevelGenerator.instance.StartCoroutine(EnemyMoveAnim(Vector3.left, enemy.transform));
                else if (CanMove(enemy.transform.position, Vector3.right))
                    LevelGenerator.instance.StartCoroutine(EnemyMoveAnim(Vector3.right, enemy.transform));
            }
        }
    }
}
