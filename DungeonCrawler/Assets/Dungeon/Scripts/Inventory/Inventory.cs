using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Inventory : MonoBehaviour {

    private List<Item> inventory = new List<Item>();
    private ItemDatabase itemDatabase;
    public InventoryUI UIInventory;
    
	// Use this for initialization  
	void Start ()
	{
	    itemDatabase = GameObject.FindGameObjectWithTag("Item Database").GetComponent<ItemDatabase>();

        UIInventory.Generate(() =>
        {
            int i = 0;
            foreach (var item in itemDatabase.Items)
            {
                UIInventory.Slots[i].GetChild(0).GetComponent<Image>().enabled = true;
                UIInventory.Slots[i].GetChild(0).GetComponent<Image>().sprite = item.Value.ItemIcon;
                i++;
            }
        });

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
