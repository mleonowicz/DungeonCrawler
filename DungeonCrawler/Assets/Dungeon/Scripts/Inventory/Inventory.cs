using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public Sprite MaskSprite;
    private bool isInEq;

    public ItemDatabase itemDatabase;
    public InventoryUI UIInventory;

    [Header("Inventory")]
    public List<Item> PlayerInventory;

    public List<Item> PlayerEquipment;

    public ItemEquipmentSlot LeftHand;
    public ItemEquipmentSlot Chest;
    public ItemEquipmentSlot RightHand;
    public ItemEquipmentSlot Helmet;

    [Header("Selection")]
    public Vector2 SelectionPos;
    public Item SelectedItem;
    public int SelectionIndex;
    private int LastSelectionIndex;
    private Vector2 LastSelectionPosition;

    public Text ItemName;
    public Text ItemStats;
    public Text ItemDesc;
    public Image ItemStatisticsObject;

    void Awake()
    {
        //itemDatabase = GameObject.FindGameObjectWithTag("Item Database").GetComponent<ItemDatabase>();
        PlayerInventory = new List<Item>();
        SelectionPos = Vector2.zero;
    }

    // Use this for initialization  
    void Start()
    {
        StartCoroutine(GenerateInventory());
    }

    IEnumerator GenerateInventory()
    {
        while (!itemDatabase.IsLoaded)
        {
            yield return new WaitForSeconds(0.01f);
        }

        //AddStartingItems(itemDatabase.Items["Wooden Shield"], itemDatabase.Items["Iron Shield"], itemDatabase.Items["Golden Shield"], itemDatabase.Items["Strengthened Wooden Shield"], itemDatabase.Items["Strengthened Iron Shield"]);
        //if (PlayerInventory[0] != null)
        //    SelectedItemImage = PlayerInventory[0];

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

        foreach (var v in UIInventory.Slots)
        {
            v.GetChild(0).GetComponent<Image>().enabled = false;
            UIInventory.Slots[i].GetChild(0).GetComponent<Image>().sprite = null;
        }

        foreach (var item in PlayerInventory)
        {
            UIInventory.Slots[i].GetChild(0).GetComponent<Image>().enabled = true;
            UIInventory.Slots[i].GetChild(0).GetComponent<Image>().sprite = item
                .ItemIcon;
            i++;
        }
    }

    public void DoIfActive()
    {


        if (Input.GetKeyDown(KeyCode.P))
            if (SelectedItem != null)
                if (isInEq)
                    UnequipItemSwitch();         
                else EquipItemSwitch();

        if (Input.GetKeyDown(KeyCode.O))
            if (SelectedItem != null)
            {
                ItemStatisticsObject.gameObject.SetActive(true);
                ShowStatistics();
            }
            else
                ItemStatisticsObject.gameObject.SetActive(false);

        if (isInEq)
            EquipmentSelectionMovement();
        else if (InventorySelectionMovement())
            SelectNewItem();

    }

    private bool InventorySelectionMovement()
    {
        foreach (var ControlScheme in GameManager.instance.ControlSchemes)
        {
            if (MoveSelectionX(ControlScheme.Left, -1)) return true;
            if (MoveSelectionX(ControlScheme.Right, 1)) return true;
            if (MoveSelectionY(ControlScheme.Up, 1)) return true;
            if (MoveSelectionY(ControlScheme.Down, -1)) return true;
        }
        return false;
    }

    private void EquipmentSelectionMovement()
    {
        foreach (var ControlScheme in GameManager.instance.ControlSchemes)
        {
            MoveSelectionEquipmentSwitch(ControlScheme.Left, -1);
            MoveSelectionEquipmentSwitch(ControlScheme.Right, 1);
        }
    }

    void MoveSelectionEquipmentSwitch(KeyCode kc, int i)
    {
        if (Input.GetKeyDown(kc))
        {
            SelectionIndex += i;
            switch (SelectionIndex)
            {
                case -1:
                    SelectionIndex = LastSelectionIndex;

                    if (SelectionIndex + 1 <= PlayerInventory.Count)
                        SelectedItem = PlayerInventory[SelectionIndex];
                    else SelectedItem = null;

                    isInEq = false;
                    UIInventory.SelectedItemImage.transform.position = LastSelectionPosition;

                    break;
                case 0:
                    MoveSelectionEquipment(LeftHand);
                    break;
                case 1:
                    MoveSelectionEquipment(Chest);
                    break;
                case 2:
                    MoveSelectionEquipment(Helmet);

                    break;
                case 3:
                    MoveSelectionEquipment(RightHand);
                    break;           
                case 4:
                    SelectionIndex = 3;
                    break;
            }
        }
    }

    void MoveSelectionEquipment(ItemEquipmentSlot lh)
    {
        SelectedItem = lh.ItemProperties;
        UIInventory.SelectedItemImage.transform.position = lh.transform.position;
    }

    bool MoveSelectionX(KeyCode kc, int i)
    {
        if (Input.GetKeyDown(kc))
        {
            if (0 <= SelectionPos.x + i)
                if (UIInventory.Size.x > SelectionPos.x + i)
                {
                    SelectionPos = new Vector2(SelectionPos.x + i, SelectionPos.y);
                    UIInventory.SelectedItemImage.transform.position +=
                        new Vector3(i * (UIInventory.LayoutGroup.cellSize.x + UIInventory.LayoutGroup.spacing.x), 0, 0);
                    return true;
                }
                else
                {
                    LastSelectionPosition = UIInventory.SelectedItemImage.transform.position;
                    LastSelectionIndex = SelectionIndex;
                    SelectionIndex = 0;
                    isInEq = true;
                    MoveSelectionEquipmentSwitch(kc, 0);                   
                }
        }
        return false;
    }

    bool MoveSelectionY(KeyCode kc, int i)
    {
        if (Input.GetKeyDown(kc))
        {
            if (UIInventory.Size.y > SelectionPos.y - i && 0 <= SelectionPos.y - i)
            {
                SelectionPos = new Vector2(SelectionPos.x, SelectionPos.y - i);
                UIInventory.SelectedItemImage.transform.position += new Vector3(0, i * (UIInventory.LayoutGroup.cellSize.y + UIInventory.LayoutGroup.spacing.y), 0);
                return true;
            }
        }
        return false;
    }

    private void SelectNewItem()
    {
        SelectionIndex = Mathf.RoundToInt(UIInventory.Size.x * SelectionPos.y + SelectionPos.x);
        if (PlayerInventory.Count > SelectionIndex)
            SelectedItem = PlayerInventory[SelectionIndex];
        else SelectedItem = null;
    }

    public void EquipItemSwitch()
    {
        switch (SelectedItem.MyItemType)
        {
            case Item.ItemType.LeftHand:
                EquipItem(LeftHand);
                break;
            case Item.ItemType.Chest:
                EquipItem(Chest);
                break;
            case Item.ItemType.RightHand:
                EquipItem(RightHand);
                break;
            case Item.ItemType.Helmet:
                EquipItem(Helmet);
                break;
        }
    }

    private void UnequipItemSwitch()
    {
        switch (SelectedItem.MyItemType)
        {
            case Item.ItemType.LeftHand:
                UnequipItem(LeftHand);
                break;
            case Item.ItemType.Chest:
                UnequipItem(Chest);
                break;
            case Item.ItemType.RightHand:
                UnequipItem(RightHand);
                break;
            case Item.ItemType.Helmet:
                UnequipItem(Helmet);
                break;
        }
    }

    private void UnequipItem(ItemEquipmentSlot lh)
    {
        PlayerInventory.Add(SelectedItem);

        lh.GetComponent<Image>().sprite = MaskSprite;
        
        SelectedItem = null;
        lh.ItemProperties = null;

        RefreshInventory();
    }

    private void EquipItem(ItemEquipmentSlot lh)
    {
        if (lh.ItemProperties == null)
        {
            lh.SetProperties(SelectedItem);

            if (SelectionIndex == PlayerInventory.Count - 1)
            {
                SelectedItem = null;
                PlayerInventory.RemoveAt(SelectionIndex);
            }
            else
            {
                PlayerInventory.RemoveAt(SelectionIndex);
                SelectedItem = PlayerInventory[SelectionIndex];
            }
        }

        else
        {
            var tem = lh.ItemProperties;

            lh.SetProperties(PlayerInventory[SelectionIndex]);
            PlayerInventory[SelectionIndex] = tem;
            SelectedItem = tem;
        }
        RefreshInventory();
    }

    private void ShowStatistics()
    {
        ItemName.text = SelectedItem.Name;
        ItemDesc.text = "\"" + SelectedItem.ItemDesc + "\"";

        switch (SelectedItem.MyItemType)
        {
            case Item.ItemType.RightHand:
                var r = SelectedItem.ItemStatistics as RightHandItem;
                if (r != null)
                    ItemStats.text = "    Attack Damage: " + r.AttacDamage + "\n" + "    Attack Speed: " + r.AttackSpeed;

                break;

            case Item.ItemType.LeftHand:
                var l = SelectedItem.ItemStatistics as LeftHandItem;
                if (l != null)
                    ItemStats.text = "    Armor: " + l.Armor + "\n" + "    Block Chance: " + l.BlockChance + "%";

                break;

            case Item.ItemType.Helmet:
                var h = SelectedItem.ItemStatistics as HelmetItem;
                if (h != null)
                    ItemStats.text = "    Armor: " + h.Armor;

                break;

            case Item.ItemType.Chest:
                var c = SelectedItem.ItemStatistics as ChestItem;
                if (c != null)
                    ItemStats.text = "    Armor: " + c.Armor;

                break;
        }
    }
}
