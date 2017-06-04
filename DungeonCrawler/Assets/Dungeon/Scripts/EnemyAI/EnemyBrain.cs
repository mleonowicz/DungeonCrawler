using System;
using UnityEngine;
using System.Collections;
using UnityEngine.Assertions;
using UnityEngine.Networking;
using Random = UnityEngine.Random;

public abstract class EnemyBrain : ScriptableObject
{
    public float PlayerDetectionRange;

    public Vector3[] Directions = { Vector3.right, Vector3.down, Vector3.left, Vector3.up }; // List for chosing directions for enemies
    public abstract void Think(Enemy enemy, Player myPlayer);

    public LayerMask layerMask;
    public LayerMask layerMaskGround = 1 << 11;

    protected bool CanMove(Vector3 currentPosition, Vector3 myVector)
    {
        if (Physics2D.OverlapPoint(currentPosition + myVector, layerMask))
        {
            return false;
        }

        foreach (var vector3 in GameManager.instance.EnemiesMovement)
        {
            if (vector3 == (currentPosition + myVector))
            {
                //Debug.Log("Nie moge");
                
                return false;
            }
        }
        return true;
    }

    protected void Movement(Transform thinkerTransform)
    {
        for (int i = 0; i < 4; i++)
        {
            int RandomDir = Random.Range(0, 4);
            var x = Directions[RandomDir];

            if (CanMove(thinkerTransform.position, x))
            {
                GameManager.instance.EnemiesMovement.Add(thinkerTransform.position + x);
                
                LevelGenerator.instance.StartCoroutine(EnemyMoveAnim(x, thinkerTransform));
                return;
            }
        }

        GameManager.instance.EnemiesMovement.Add(thinkerTransform.position);

    }

    public IEnumerator EnemyMoveAnim(Vector3 dir, Transform thinkerTransform)
    {
        var time = 0f;
        var startPos = thinkerTransform.position;
        var endPos = thinkerTransform.position += dir;

        while (time < 1)
        {
            
            time += Time.deltaTime * 5;
            thinkerTransform.position = Vector3.Lerp(startPos, endPos, time);

            yield return new WaitForEndOfFrame();

        }
        CheckGround(thinkerTransform);
    
    }

    public void CheckGround(Transform thinkerTransform)
    {
        Collider2D hit = Physics2D.OverlapPoint(thinkerTransform.position, layerMaskGround);

        thinkerTransform.GetComponent<SpriteRenderer>().enabled = hit.gameObject.GetComponent<SpriteRenderer>().enabled;
        // dlaczego nie działasz ;_;
    }
}