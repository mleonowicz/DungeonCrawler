using System;
using UnityEngine;
using System.Collections;

[Serializable]
public struct LikeControlScheme
{
    public KeyCode Left;
    public KeyCode Right;
    public KeyCode Up;
    public KeyCode Down;
}

public class Player : MonoBehaviour
{
    public LikeControlScheme[] ControlSchemes;
    public PlayerData PlayerData;

    public int CurrentHP;
    public int CurrentMP;

    void Start()
    {
        CurrentHP = PlayerData.MaxHP * PlayerData.Str;
        CurrentMP = PlayerData.MaxMP * PlayerData.Int;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool Movement()
    {
        foreach (var ControlScheme in ControlSchemes)
        {
            if (Move(ControlScheme.Left, Vector3.left)) return true;
            if (Move(ControlScheme.Right, Vector3.right)) return true;
            if (Move(ControlScheme.Up, Vector3.up)) return true;
            if (Move(ControlScheme.Down, Vector3.down)) return true;
            // return false;
        }
        return false;
    }

    private bool Move(KeyCode kc, Vector3 dir)
    {
        if (Input.GetKeyDown(kc))
        {
            if (!CanMove(dir))
                return false;
            transform.localPosition += dir;
            return true;
        }
        return false;
    }

    private bool CanMove(Vector3 myVector)
    {
        if (Physics2D.OverlapPoint(transform.localPosition + (myVector * 0.5f) + Vector3.one * 0.5f))
        // if (Physics2D.Raycast(transform.localPosition + Vector3.up * 0.5f, myVector, 0.5f))
        {
            // Debug.Log("Nie moszna");
            return false;
        }
        return true;
    }
}