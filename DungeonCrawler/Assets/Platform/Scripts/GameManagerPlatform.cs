using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManagerPlatform : MonoBehaviour
{
    public PlayerPlatform MyPlayer;
    public EnemyPlatform MyEnemy;

    [SerializeField]
    private Text HP;
    [SerializeField]
    private Text MP;

    void Start()
    {
        
    }


    void Update()
    {
        //UpdateUI();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            MyPlayer.MyPlayerStats.HP -= 2;
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            ReturnToDungeon();
        }
    }

    void UpdateUI()
    {
        HP.text = MyPlayer.MyPlayerStats.HP.ToString();
        MP.text = MyPlayer.MyPlayerStats.MP.ToString();
    }

    void ReturnToDungeon()
    {
        GameData.MyPlayerStats = MyPlayer.MyPlayerStats;       
        SceneManager.UnloadSceneAsync(1);

        GameData.SceneObject.SetActive(true);
    }
}