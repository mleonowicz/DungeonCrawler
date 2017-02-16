using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{

    private List<Item> inventory = new List<Item>();
    private ItemDatabase itemDatabase;
    public InventoryUI UIInventory;
    
	// Use this for initialization  
	void Start ()
	{
	    itemDatabase = GameObject.FindGameObjectWithTag("Item Database").GetComponent<ItemDatabase>();

	    StartCoroutine(GenerateInventory());
	}

    IEnumerator GenerateInventory()
    {
        while (!itemDatabase.IsLoaded)
        {
            yield return new WaitForSeconds(0.01f);
        }

        UIInventory.Generate(() =>
        {
            int i = 0;
            foreach (var item in itemDatabase.Items) // TODO zmienic na start items
            {
                UIInventory.Slots[i].GetChild(0).GetComponent<Image>().enabled = true;
                UIInventory.Slots[i].GetChild(0).GetComponent<Image>().sprite = item.Value.ItemIcon;
                i++;
            }
        });

        yield break;
    }

	// Update is called once per frame
	void Update () {
	
	}
}
