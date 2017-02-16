using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
//[CreateAssetMenu]
public class ItemDatabase : MonoBehaviour
{
    public bool IsLoaded = false;

    public void Awake()
    {
        Initialize();
    }

    public Dictionary<string, Item> Items;
    public string SubFolder;

    //  public abstract void Load();
    [ContextMenu("Initialize")]
    public void Initialize()
    {
        Items = new Dictionary<string, Item>();
        foreach (var entity in Resources.LoadAll<Item>(SubFolder))
        {
            Items.Add(entity.name, entity);
        }
        IsLoaded = true;
    }

    public Item GetItem(string name)
    {
        return Items[name]; //.FirstOrDefault(t => t.Value.name == name).Value;
    }

    //    public TileInfo GetTile(string name)
    //    {
    //        return Items[name].Tile;
    //    }

    //    public TileEntity GetEntity(TileInfo element)
    //    {
    //        return Items.FirstOrDefault(t => t.Value.Tile == element).Value;
    //    }


}
