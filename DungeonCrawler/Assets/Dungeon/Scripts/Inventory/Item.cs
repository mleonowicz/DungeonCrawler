using UnityEngine;
using System.Collections;

[CreateAssetMenu]
public class Item : ScriptableObject
{
    public string Name;
    public string ItemDesc;
    public Sprite ItemIcon;
    public int AttackItem;
    public ItemType MyItemType;

    public enum ItemType
    {
        Weapon, 
        Consumable
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
