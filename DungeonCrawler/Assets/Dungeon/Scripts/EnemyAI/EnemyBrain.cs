using UnityEngine;
using System.Collections;

public abstract class EnemyBrain : ScriptableObject
{
    public float PlayerDetectionRange;

    public abstract void Think(Enemy enemy, Player myPlayer);

    int layerMask = 1 << 8;

    protected bool CanMove(Vector3 currentPosition, Vector3 myVector)
    {
        if (Physics2D.OverlapPoint(currentPosition + myVector + Vector3.one * 0.5f, layerMask))
        {
            // Debug.Log("Cant'Move");    
            
            return false; 
        }
        return true;
    }

    protected void Movement(Transform thinkerTransform)
    {
        int d = Random.Range(1, 5);
        // Debug.Log(d);
        switch (d)
        {
            case 1:
                if (CanMove(thinkerTransform.position, Vector3.left))
                    thinkerTransform.position += Vector3.left;
                    
                break;

            case 2:
                if (CanMove(thinkerTransform.position, Vector3.right))
                    thinkerTransform.position += Vector3.right;
                break;

            case 3:
                if (CanMove(thinkerTransform.position, Vector3.up))
                    thinkerTransform.position += Vector3.up;
                break;

            case 4:
                if (CanMove(thinkerTransform.position, Vector3.down))
                    thinkerTransform.position += Vector3.down;
                break;
        }
    }

    public IEnumerator EnemyMoveAnim(Vector3 dir, Transform thinkerTransform)
    {
        var time = 0f;
        var startPos = thinkerTransform.position;
        var endPos = thinkerTransform.position += dir;

        while (time < 1)
        {
            Debug.Log(startPos + " " + endPos);
            time += Time.deltaTime * 5;
            thinkerTransform.position = Vector3.Lerp(startPos, endPos, time);

            yield return new WaitForEndOfFrame();
        }
        //transform.position = new Vector3(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y), transform.position.z);
    }



}
