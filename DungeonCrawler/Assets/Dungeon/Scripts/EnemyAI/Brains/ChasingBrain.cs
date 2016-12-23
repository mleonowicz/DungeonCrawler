using UnityEngine;
using System.Collections;

[CreateAssetMenu]
public class ChasingBrain : EnemyBrain
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

            if (Mathf.Abs(dir.x) >= Mathf.Abs(dir.y))
            {
                dir = new Vector3(Mathf.Sign(dir.x), 0);
                if (CanMove(enemy.transform.position, dir))
                    enemy.transform.position += dir;
                else if (Vector3.Distance(myPlayer.transform.position, enemy.transform.position + Vector3.up) >=
                    Vector3.Distance(myPlayer.transform.position, enemy.transform.position + Vector3.down))
                {
                    dir = Vector3.down;
                    if (CanMove(enemy.transform.position, dir))
                        enemy.transform.position += dir;                  
                }
                else if (CanMove(enemy.transform.position, Vector3.up))
                    enemy.transform.position += Vector3.up; 

            }
            else
            {
                dir = new Vector3(0, Mathf.Sign(dir.y));
                if (CanMove(enemy.transform.position, dir))
                    enemy.transform.position += dir;
                else if (Vector3.Distance(myPlayer.transform.position, enemy.transform.position + Vector3.left) >=
                    Vector3.Distance(myPlayer.transform.position, enemy.transform.position + Vector3.right))
                {
                    dir = Vector3.right;
                    if (CanMove(enemy.transform.position, dir))
                        enemy.transform.position += dir;
                }
                else if (CanMove(enemy.transform.position, Vector3.left))
                        enemy.transform.position += Vector3.left;
            }
        }
    }
}
