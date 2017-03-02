using UnityEngine;
using System.Collections;

[CreateAssetMenu]
public class ChasingBrain : EnemyBrain
{
    public override void Think(Enemy enemy, Player myPlayer)
    {
        Vector3 dir = Vector3.up;

        // var disc = Vector3.Distance(myPlayer.transform.position, enemy.transform.position);
    
        if (enemy.transform.position == myPlayer.transform.position)
        {
            //Debug.Log("Topkek");
            return;
        }

        if (Vector3.Distance(enemy.transform.position, myPlayer.transform.position) > PlayerDetectionRange)
        {
            Movement(enemy.transform);
        }
        else
        {
            dir = myPlayer.transform.position - enemy.transform.position;
            dir = dir.normalized;


            // TODO 

            /* 
            var cpV = new Vector3(Mathf.Abs(v.x), Mathf.Abs(v.y)); // Made using Sinus <3

            var sin1 = Vector3.Angle(new Vector3(0, 1), cpV);
            var sin2 = Vector3.Angle(new Vector3(1, 0), cpV); */

            // Using LevelGenerator Instanse because this class inheritates from ScriptableObject and it doenst have "Start Courutine" Function

            if (Mathf.Abs(dir.x) >= Mathf.Abs(dir.y))
            {
                dir = new Vector3(Mathf.Sign(dir.x), 0);
                if (CanMove(enemy.transform.position, dir))
                    LevelGenerator.instance.StartCoroutine(EnemyMoveAnim(dir, enemy.transform));
                else if (Vector3.Distance(myPlayer.transform.position, enemy.transform.position + Vector3.up) >=
                    Vector3.Distance(myPlayer.transform.position, enemy.transform.position + Vector3.down))
                {
                    if (CanMove(enemy.transform.position, dir))
                    {
                        dir = Vector3.down;
                        LevelGenerator.instance.StartCoroutine(EnemyMoveAnim(dir, enemy.transform));
                    }
                }
                else if (CanMove(enemy.transform.position, Vector3.up))
                {
                    dir = Vector3.up;
                    LevelGenerator.instance.StartCoroutine(EnemyMoveAnim(dir, enemy.transform));
                }
            }
            else
            {
                dir = new Vector3(0, Mathf.Sign(dir.y));
                if (CanMove(enemy.transform.position, dir))
                    LevelGenerator.instance.StartCoroutine(EnemyMoveAnim(dir, enemy.transform));
                else if (Vector3.Distance(myPlayer.transform.position, enemy.transform.position + Vector3.left) >=
                    Vector3.Distance(myPlayer.transform.position, enemy.transform.position + Vector3.right))
                {
                    dir = Vector3.right;
                    if (CanMove(enemy.transform.position, dir))
                        LevelGenerator.instance.StartCoroutine(EnemyMoveAnim(dir, enemy.transform));
                }
                else if (CanMove(enemy.transform.position, Vector3.left))
                {
                    dir = Vector3.left;
                    
                    LevelGenerator.instance.StartCoroutine(EnemyMoveAnim(dir, enemy.transform));

                }
            }
        }

        
//        GameManager.instance.EnemiesMovement.Add(enemy.transform.position + dir);
    }
}
