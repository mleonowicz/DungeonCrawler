using UnityEngine;

[CreateAssetMenu]
public class LevelGenaratorSettings : ScriptableObject
{
    public int Seed;

    [Header("Prefabs")]
    public GameObject[] FloorTiles;
    public GameObject[] WallTiles;
    public GameObject[] Items;
    public GameObject[] Enemies;
    public GameObject Exit;
    public GameObject Ceiling;

    [Header("Object Ammount")]

    public int NumberOfTiles;
    public int TileSize;
    public int NumberOfItems;
    public int NumberOfEnemies;

    [Header("Move Chance")]

    public float ChanceUp;
    public float ChanceRight;
    public float ChanceDown;

    [Header("Extra Walls")]

    public float ExtraWallX = 4;
    public float ExtraWallY = 4;

    [ContextMenu("Random Seed")]
    public void SetRandomSeed()
    {
        Seed = Random.Range(0, int.MaxValue);
    }

}