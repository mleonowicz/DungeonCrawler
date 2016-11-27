using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System.Linq;

public class LevelGenerator : MonoBehaviour
{
    private List<Vector3> CreatedTiles;
    private int LastPosition;

    [SerializeField]
    private int NumberOfTiles;
    [SerializeField]
    private int TileSize;

    [SerializeField]
    private GameObject[] FloorTiles;
    [SerializeField]
    private GameObject[] WallTiles;

    [SerializeField]
    private float ChanceUp;
    [SerializeField]
    private float ChanceRight;
    [SerializeField]
    private float ChanceDown;

    public float MaxY = 0;
    public float MinY = 999;
    public float MaxX = 0;
    public float MinX = 999;
    public float XWallsNumber;
    public float YWallsNumber;
    private float ExtraWallX = 4;
    private float ExtraWallY = 4;


    void Awake()
    {
        CreatedTiles = new List<Vector3>();
    }

    void Start()
    {
        GenerateLevel();
    }

    void GenerateLevel()
    {
        for (int i = 0; i < NumberOfTiles; i++)
        {
            InstantiateTile(Random.Range(0, FloorTiles.Length));
            ChanceGenerator(Random.Range(0f, 1f));          
        }

        WallsGenerator();        
    }

    void ChanceGenerator(float rnd)
    {

        if (rnd < ChanceUp)
            MoveGenerator(1);
        else if (rnd < ChanceRight)
            MoveGenerator(2);
        else if (rnd < ChanceDown)
            MoveGenerator(-1);
        else MoveGenerator(-2);
    }

    void InstantiateTile(int tileIndex)
    {
        if (!CreatedTiles.Contains(transform.position))
        {
            Instantiate(FloorTiles[tileIndex], transform.position, Quaternion.identity);
            CreatedTiles.Add(transform.position);
        }
        else NumberOfTiles++;
    }

    void MoveGenerator(int rnd)
    {
        if (LastPosition * -1 == rnd)
          return;

        switch (rnd)
        {
            case 1:
                transform.position = new Vector3(transform.position.x, transform.position.y + TileSize);
                break;

            case 2:
                transform.position = new Vector3(transform.position.x + TileSize, transform.position.y);
                break;

            case -1:
                transform.position = new Vector3(transform.position.x, transform.position.y - TileSize);
                break;

            case -2:
                transform.position = new Vector3(transform.position.x - TileSize, transform.position.y);
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

        XWallsNumber = (MaxX - MinX) + ExtraWallX;
        YWallsNumber = (MaxY - MinY) + ExtraWallY;
    }

    void CreatingWalls()
    {
        for (int x = 1; x < XWallsNumber; x++)
        {
            for (int y = 1; y < YWallsNumber; y++)
            {
                if (!CreatedTiles.Contains(new Vector3(MinX + x - ExtraWallX / 2,MinY + y - ExtraWallY / 2)))
                    Instantiate(WallTiles[Random.Range(0, WallTiles.Length)], new Vector3(MinX + x - ExtraWallX / 2, MinY + y - ExtraWallY / 2), Quaternion.identity);

            }
        }
    }
}
