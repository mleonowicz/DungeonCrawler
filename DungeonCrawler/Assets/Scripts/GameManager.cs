using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Player myPlayer;
    [SerializeField]
    private Text Turn;


    public List<GameObject> Enemies;

    private int TurnCount = 1;

    void Start()
    {
        UpdateUI();
    }

    // Update is called once per frame
    void Update()
    {
        if (myPlayer.Movement())
        {
            TurnCount++;
            UpdateUI();
            foreach (var enemy in Enemies)
            {
                MoveGenerator(enemy, Random.Range(0, 4));
            }
        }

    }

    void MoveGenerator(GameObject toMove, int rnd)
    {
        if (Vector3.Distance(myPlayer.transform.position, toMove.transform.position) < 5)
        {
            var dir = myPlayer.transform.position - toMove.transform.position;
            dir = dir.normalized;
            if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
            {
                dir = new Vector3(Mathf.Sign(dir.x), 0);
            }
            else
            {
                dir = new Vector3(0, Mathf.Sign(dir.y));

            }
            if (Move(toMove, dir)) return;
        }
        switch (rnd)
        {
            case 0:
                Move(toMove, Vector3.left);
                break;
            case 1:
                Move(toMove, Vector3.right);
                break;

            case 2:
                Move(toMove, Vector3.up);
                break;

            case 3:
                Move(toMove, Vector3.down);
                break;
        }

    }

    private bool Move(GameObject objectToMove, Vector3 dir)
    {
        if (CanMove(objectToMove, dir))
        {
            objectToMove.transform.localPosition += dir;
            return true;
        }
        return false;
    }

    private bool CanMove(GameObject objectToMove, Vector3 myVector)
    {
        if (Physics2D.OverlapPoint(objectToMove.transform.localPosition + (myVector * 0.5f) + Vector3.one * 0.5f))
        // if (Physics2D.Raycast(transform.localPosition + Vector3.up * 0.5f, myVector, 0.5f))
        {
            Debug.Log("KURO ! Nie moszna");
            return false;
        }
        return true;
    }

    void UpdateUI()
    {
        Turn.text = "Turn: " + TurnCount;
    }
    public void OnDrawGizmos()
    {
        foreach (var enemy in Enemies)
        {
            Gizmos.DrawSphere(enemy.transform.localPosition + (Vector3.up * 0.5f) + Vector3.one * 0.5f, 0.2f);
            Gizmos.DrawSphere(enemy.transform.localPosition + (Vector3.down * 0.5f) + Vector3.one * 0.5f, 0.2f);
            Gizmos.DrawSphere(enemy.transform.localPosition + (Vector3.left * 0.5f) + Vector3.one * 0.5f, 0.2f);
            Gizmos.DrawSphere(enemy.transform.localPosition + (Vector3.right * 0.5f) + Vector3.one * 0.5f, 0.2f);
        }
    }

}
