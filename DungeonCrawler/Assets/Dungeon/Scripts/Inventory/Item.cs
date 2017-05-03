using UnityEngine;
using System.Collections;

[CreateAssetMenu]
public class Item : ScriptableObject
{
    public string Name;
    public string ItemDesc;
    public Sprite ItemIcon;

    public ItemStatistics ItemStatistics;

    public ItemType MyItemType;

    public enum ItemType
    {
        LeftHand,
        RightHand,
        Helmet,
        Chest
    }
}