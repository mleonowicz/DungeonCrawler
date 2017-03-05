using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
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

    public static GameManager instance;

    public List<Vector3> EnemiesMovement;
    public List<GameObject> Enemies;

    private int TurnCount = 1;

    void Awake()
    {
        instance = this;
        EnemiesMovement = new List<Vector3>();
    }

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
            EnemiesMovement = new List<Vector3>();

            foreach (var enemy in Enemies)
            {
                enemy.GetComponent<Enemy>().MakeTurn(myPlayer);
            }
        }
        UpdateUI();
        
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
    //        Gizmos.DrawSphere(enemy.transform.localPosition + (Vector3.up) + Vector3.one * 0.5f, 0.2f);
    //        Gizmos.DrawSphere(enemy.transform.localPosition + (Vector3.down) + Vector3.one * 0.5f, 0.2f);
    //        Gizmos.DrawSphere(enemy.transform.localPosition + (Vector3.left) + Vector3.one * 0.5f, 0.2f);
    //        Gizmos.DrawSphere(enemy.transform.localPosition + (Vector3.right) + Vector3.one * 0.5f, 0.2f);
    //    }
    //}
}
