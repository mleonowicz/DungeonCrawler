using UnityEngine;
using System.Collections;
using System;

public class MeleeState : IEnemyPlatformState {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Execute()
    {
        throw new System.NotImplementedException();
    }

    public void Enter(EnemyPlatform enemy)
    {
        throw new System.NotImplementedException();
    }

    public void Exit()
    {
        throw new System.NotImplementedException();
    }

    public void OnTriggerEnter(Collider2D other)
    {
        throw new NotImplementedException();
    }
}
