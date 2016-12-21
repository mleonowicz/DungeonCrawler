using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine.UI;
using Random = UnityEngine.Random;

public class LevelGenerator : MonoBehaviour
{
    public LevelGenaratorSettings Settings;

    private List<Vector3> CreatedTiles;
    private List<Vector3> Borders;
    private int LastPosition;

    private Vector3 PlayerPosition;
    private Vector3 ExitPosition;

    public GameObject WallParent;
    public GameObject TileParent;
    public GameObject ItemParent;
    public GameObject EnemyParent;

    public GameObject Player;

    private int AdditionalTiles = 0;

    private float MaxY = 0;
    private float MinY = 999;
    private float MaxX = 0;
    private float MinX = 999;
    private float XWallsNumber;
    private float YWallsNumber;

    void Awake()
    {
        CreatedTiles = new List<Vector3>();
        Borders = new List<Vector3>();
    }

    void Start()
    {
        GenerateSeed();
        GenerateLevel();
        SettingBorders();
        GeneratingStartPositions();
        GeneratingItems();
        GeneratingEnemies();
        // Destroy(gameObject);
    }

    void GenerateLevel()
    {
        for (int i = 0; i < Settings.NumberOfTiles + AdditionalTiles; i++)
        {
            InstantiateTile(Random.Range(0, Settings.FloorTiles.Length));
            ChanceGenerator(Random.Range(0f, 1f));
        }

        WallsGenerator();
    }

    void ChanceGenerator(float rnd)
    {

        if (rnd < Settings.ChanceUp)
            MoveGenerator(1);
        else if (rnd < Settings.ChanceRight)
            MoveGenerator(2);
        else if (rnd < Settings.ChanceDown)
            MoveGenerator(-1);
        else MoveGenerator(-2);
    }

    void InstantiateTile(int tileIndex)
    {
        if (!CreatedTiles.Contains(transform.position))
        {
            var g = Instantiate(Settings.FloorTiles[tileIndex], transform.position, Quaternion.identity) as GameObject;
            CreatedTiles.Add(transform.position);
            g.transform.SetParent(TileParent.transform, false);
        }
        else AdditionalTiles++;
    }

    void MoveGenerator(int rnd)
    {
        if (LastPosition * -1 == rnd)
            return;

        switch (rnd)
        {
            case 1:
                transform.position = new Vector3(transform.position.x, transform.position.y + Settings.TileSize);
                break;

            case 2:
                transform.position = new Vector3(transform.position.x + Settings.TileSize, transform.position.y);
                break;

            case -1:
                transform.position = new Vector3(transform.position.x, transform.position.y - Settings.TileSize);
                break;

            case -2:
                transform.position = new Vector3(transform.position.x - Settings.TileSize, transform.position.y);
                break;
        }
        LastPosition = rnd;
        // Debug.Log(rnd);
    }

    void WallsGenerator()
    {
        CountingWallValues();
        CreatingWalls();

    }

    void CountingWallValues()
    {
        for (int i = 0; i < CreatedTiles.Count; i++)
        {
            if (CreatedTiles[i].y < MinY)
                MinY = CreatedTiles[i].y;

            if (CreatedTiles[i].y > MaxY)
                MaxY = CreatedTiles[i].y;

            if (CreatedTiles[i].x < MinX)
                MinX = CreatedTiles[i].x;

            if (CreatedTiles[i].x > MaxX)
                MaxX = CreatedTiles[i].x;
        }

        XWallsNumber = (MaxX - MinX) + Settings.ExtraWallX;
        YWallsNumber = (MaxY - MinY) + Settings.ExtraWallY;
    }

    void CreatingWalls()
    {
        for (int x = 1; x < XWallsNumber; x++)
        {
            for (int y = 1; y < YWallsNumber; y++)
            {
                if (!CreatedTiles.Contains(new Vector3(MinX + x - Settings.ExtraWallX / 2, MinY + y - Settings.ExtraWallY / 2)))
                {
                    var g = Instantiate(Settings.WallTiles[Random.Range(0, Settings.WallTiles.Length)],
                        new Vector3(MinX + x - Settings.ExtraWallX / 2, MinY + y - Settings.ExtraWallY / 2),
                        Quaternion.identity) as GameObject;
                    g.transform.SetParent(WallParent.transform, false);
                }
            }
        }
    }

    public int CurrentSeed;

    void GenerateSeed()
    {
        CurrentSeed = Settings.Seed == 0 ? Random.Range(Int32.MinValue, Int32.MaxValue) : Settings.Seed;
        Random.InitState(CurrentSeed);

    }
    /* IEnumerator SeedCoroutine()
    {
        mySeed.text = Seed.ToString();
        yield return new WaitForSeconds(5);

    } */

    void GeneratingStartPositions()
    {
        if (Borders[0].y > Borders[1].y)
            PlayerPosition = Borders[1];

        else PlayerPosition = Borders[0];

        List<Vector3> sortedList = new List<Vector3>(CreatedTiles.OrderByDescending(v => Vector3.Distance(PlayerPosition, v)));
        ExitPosition = sortedList[0];

        Player.transform.position = PlayerPosition;
        //Instantiate(Player, PlayerPosition, Quaternion.identity);
        Instantiate(Settings.Exit, ExitPosition, Quaternion.identity);
    }

    void SettingBorders()
    {
        Borders.Add(CreatedTiles.Find(v => v.x == MaxX));
        Borders.Add(CreatedTiles.Find(v => v.x == MinX));
    }

    void GeneratingItems()
    {
        for (int i = 0; i < Settings.NumberOfItems; i++)
        {
            var x = Instantiate(Settings.Items[Random.Range(0, Settings.Items.Length)], CreatedTiles[Random.Range(0, CreatedTiles.Count)],
                Quaternion.identity) as GameObject;

            x.transform.SetParent(ItemParent.transform);
        }
    }

    void GeneratingEnemies()
    {
        var gm = FindObjectOfType<GameManager>();
        for (int i = 0; i < Settings.NumberOfEnemies; i++)
        {
            var x = Instantiate(Settings.Enemies[Random.Range(0, Settings.Enemies.Length)], CreatedTiles[Random.Range(0, CreatedTiles.Count)],
                Quaternion.identity) as GameObject;

            x.transform.SetParent(EnemyParent.transform);
            gm.Enemies.Add(x);
        }
    }

#if UNITY_EDITOR
    [ContextMenu("Save Seed")]
    public void SaveCurrentSeed()
    {
        AssetDatabase.CopyAsset(AssetDatabase.GetAssetPath(Settings), "Assets\\Settings\\" + CurrentSeed + ".asset");
        var newSettings = AssetDatabase.LoadAssetAtPath<LevelGenaratorSettings>("Assets\\Settings\\" + CurrentSeed + ".asset");

        newSettings.Seed = CurrentSeed;

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        if (Settings.Seed == 0)
        {
            EditorApplication.isPlaying = false;
            //    EditorApplication.isPlaying = true;
        }
    }
#endif
}
