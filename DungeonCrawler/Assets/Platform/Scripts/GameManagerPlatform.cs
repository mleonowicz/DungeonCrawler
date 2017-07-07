using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManagerPlatform : MonoBehaviour
{
    public PlayerPlatform MyPlayer;

    [SerializeField]
    private Text HP;
    [SerializeField]
    private Text MP;

    void Start()
    {
        
    }


    void Update()
    {
        UpdateUI();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            MyPlayer.MyPlayerStats.CurrentHP -= 2;
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            ReturnToDungeon();
        }
    }

    void UpdateUI()
    {
        HP.text = MyPlayer.MyPlayerStats.CurrentHP.ToString();
        MP.text = MyPlayer.MyPlayerStats.CurrentMP.ToString();
    }

    void ReturnToDungeon()
    {
        GameData.MyPlayerStats = MyPlayer.MyPlayerStats;       
        SceneManager.UnloadSceneAsync(1);

        GameData.SceneObject.SetActive(true);
    }
}