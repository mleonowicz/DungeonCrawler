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
            // Debug.Log("Cant'Move");    
            GameManager.instance.EnemiesMovement.Add(currentPosition);
            return false;
        }

        //Debug.Log("Moves: "+GameManager.instance.EnemiesMovement.Count);

        foreach (var vector3 in GameManager.instance.EnemiesMovement)
        {
            //Debug.Log(vector3);
            if (vector3 == (currentPosition + myVector))
            {
                Debug.Log("Nie moge");

                return false;
            }
        }

        GameManager.instance.EnemiesMovement.Add(currentPosition + myVector);


        //Debug.Log(currentPosition + myVector);
        return true;
    }

    protected void Movement(Transform thinkerTransform)
    {
        int RandomDir = Random.Range(0, 4);

        // Debug.Log(d); // TODO naprawić dać courutiny

        var x = Directions[RandomDir];

        if (CanMove(thinkerTransform.position, x))
        {

            // GameManager.instance.EnemiesMovement.Add(thinkerTransform.position + Vector3.left);
            LevelGenerator.instance.StartCoroutine(EnemyMoveAnim(x, thinkerTransform));
        }
    }

    public IEnumerator EnemyMoveAnim(Vector3 dir, Transform thinkerTransform)
    {
        var time = 0f;
        var startPos = thinkerTransform.position;
        var endPos = thinkerTransform.position += dir;

        while (time < 1)
        {
            //Debug.Log(startPos + " " + endPos);
            time += Time.deltaTime * 5;
            thinkerTransform.position = Vector3.Lerp(startPos, endPos, time);

            yield return new WaitForEndOfFrame();

        }
        CheckGround(thinkerTransform);

        // if(callback!=null)callback.Invoke();
        //transform.position = new Vector3(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y), transform.position.z);
    }

    public void CheckGround(Transform thinkerTransform)
    {
        Collider2D hit = Physics2D.OverlapPoint(thinkerTransform.position, layerMaskGround);

        //Debug.Log(hit.name + " "+hit.gameObject.GetComponent<SpriteRenderer>().enabled);
        //Debug.Log(thinkerTransform);
        thinkerTransform.GetComponent<SpriteRenderer>().enabled = hit.gameObject.GetComponent<SpriteRenderer>().enabled;
        // dlaczego nie działasz ;_;
    }
}