using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public enum Slot
{
    leftHand, chest, helmet, rightHand, consumable
}

[CreateAssetMenu]
public class Item : ScriptableObject
{
    public string Name;
    public string ItemDesc;
    public Sprite ItemIcon;
    public bool Consumable;
    public bool ShowStats;

    public List<Stat> Stats;

    public Slot Slot;

    public string GetStats()
    {
        string s = "";
        foreach (var stat in Stats)
        {
            s += stat.ToString();
        }
        return s;
    }
}