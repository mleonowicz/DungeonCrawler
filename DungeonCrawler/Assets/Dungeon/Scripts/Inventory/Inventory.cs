using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{

    public List<Item> PlayerInventory;
    private ItemDatabase itemDatabase;
    public InventoryUI UIInventory;

    void Awake()
    {
	    itemDatabase = GameObject.FindGameObjectWithTag("Item Database").GetComponent<ItemDatabase>();
        PlayerInventory = new List<Item>();
    }

    // Use this for initialization  
    void Start ()
	{
	    StartCoroutine(GenerateInventory());
	}

    IEnumerator GenerateInventory()
    {
        while (!itemDatabase.IsLoaded)
        {
            yield return new WaitForSeconds(0.01f);
        }

        AddStartingItems(itemDatabase.Items["Apple"], itemDatabase.Items["Apple"]);

        UIInventory.Generate(() =>
        {
            int i = 0;
            foreach (var item in PlayerInventory)
            {
                UIInventory.Slots[i].GetChild(0).GetComponent<Image>().enabled = true;
                UIInventory.Slots[i].GetChild(0).GetComponent<Image>().sprite = item.ItemIcon;
                i++;
            }
        });

        yield break;
    }

    void AddStartingItems(params Item[] item)
    {
        foreach (var i in item)
        {
            PlayerInventory.Add(i);
        }
    }

	// Update is called once per frame

    public void AddItem(Item item)
    {
        PlayerInventory.Add(item);
        RefreshInventory();
    }

    void RefreshInventory()
    {
        int i = 0;
        foreach (var item in PlayerInventory)
        {
            UIInventory.Slots[i].GetChild(0).GetComponent<Image>().enabled = true;
            UIInventory.Slots[i].GetChild(0).GetComponent<Image>().sprite = item.ItemIcon;
            i++;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
           AddItem(itemDatabase.Items["Shirt"]);
    }
}
