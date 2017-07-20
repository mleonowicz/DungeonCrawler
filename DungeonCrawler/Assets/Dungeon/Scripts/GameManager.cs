using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[Serializable]
public struct LikeControlScheme
{
    public KeyCode Left;
    public KeyCode Right;
    public KeyCode Up;
    public KeyCode Down;
}

public class GameManager : MonoBehaviour
{
    public LikeControlScheme[] ControlSchemes;

    [SerializeField]
    private Player myPlayer;

    public GameObject CanvasObject;

    public Text Turn;
    public Text HP;
    public Text MP;

    public GUITexture Overlay;
    public GameObject SceneObject;

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
        Overlay.pixelInset = new Rect(0, 0, Screen.width, Screen.height);
        UpdateUI();
    }

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
        myPlayer.transform.name = myPlayer.Name;
        HP.text = myPlayer.PlayerStats.HP.ToString();
        MP.text = myPlayer.PlayerStats.MP.ToString();
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

    public IEnumerator FadeOutAnimation()
    {
        CanvasObject.SetActive(false);
        
        Overlay.color = Color.clear;
        float progress = 0.0f;
        while (progress < 1.0f)
        {
            Overlay.color = Color.Lerp(Color.clear, Color.black, progress);
            progress += Time.deltaTime;
            yield return null;
        }
        Overlay.color = Color.black;

        LoadPlatformScene();
    }

    private void LoadPlatformScene()
    {      
        GameData.SceneObject = SceneObject;
        SceneObject.SetActive(false);

        SceneManager.LoadScene(1, LoadSceneMode.Additive);        
    }

    public IEnumerator FadeInAnimation()
    {
        Overlay.color = Color.black;
        float progress = 0.0f;
        while (progress < 1.0f)
        {
            Overlay.color = Color.Lerp(Color.black, Color.clear, progress);
            progress += Time.deltaTime;
            yield return null;
        }
        Overlay.color = Color.clear;

        myPlayer.inFight = false;
        CanvasObject.SetActive(true);
    }
}