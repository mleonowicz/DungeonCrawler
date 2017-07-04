using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class ItemDatabase : MonoBehaviour
{
    public bool IsLoaded = false;

    public Dictionary<string, Item> Items;
    public string SubFolder;

    public void Awake()
    {
        Initialize();
    }

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
        return Items[name]; 
    }
}
