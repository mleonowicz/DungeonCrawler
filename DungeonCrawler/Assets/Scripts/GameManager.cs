﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Player myPlayer;
    [SerializeField]
    private Text Turn;
    [SerializeField]
    private Text HP;
    [SerializeField]
    private Text MP;

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
                enemy.GetComponent<Enemy>().MakeTurn(myPlayer);
            }

        }

    }
    void UpdateUI()
    {
        Turn.text = "Turn: " + TurnCount;
        myPlayer.transform.name = myPlayer.PlayerData.Name;
        HP.text = myPlayer.CurrentHP.ToString();
        MP.text = myPlayer.CurrentMP.ToString();
    }

    //public void OnDrawGizmos()
    //{
    //    foreach (var enemy in Enemies)
    //    {
    //        Gizmos.DrawSphere(enemy.transform.localPosition + (Vector3.up * 0.5f) + Vector3.one * 0.5f, 0.2f);
    //        Gizmos.DrawSphere(enemy.transform.localPosition + (Vector3.down * 0.5f) + Vector3.one * 0.5f, 0.2f);
    //        Gizmos.DrawSphere(enemy.transform.localPosition + (Vector3.left * 0.5f) + Vector3.one * 0.5f, 0.2f);
    //        Gizmos.DrawSphere(enemy.transform.localPosition + (Vector3.right * 0.5f) + Vector3.one * 0.5f, 0.2f);
    //    }
    //}
}
