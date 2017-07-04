using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    private Player myPlayer;
    public Sprite MaskSprite;
    private bool isInEq;

    public ItemDatabase itemDatabase;
    public InventoryUI UIInventory;

    [Header("Inventory")]
    public List<Item> PlayerInventory;

    public List<ItemEquipmentSlot> EquipmentSlots;

    [Header("Selection")]
    public Vector2 SelectionPos;
    public Item SelectedItem;
    public int SelectionIndex;
    public int LastSelectionIndex;
    public Vector2 LastSelectionPosition;

    private Item ShownItem;

    public Text ItemName;
    public Text ItemStats;
    public Text ItemDesc;
    public Image ItemStatisticsObject;

    void Awake()
    {
        myPlayer = GetComponent<Player>();
        
        PlayerInventory = new List<Item>();
        SelectionPos = Vector2.zero;
    }

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

    public void AddItem(Item item)
    {
        PlayerInventory.Add(item);
        RefreshInventory();
    }

    void RefreshInventory()
    {
        int i = 0;

        foreach (var v in UIInventory.Slots)
            v.GetChild(0).GetComponent<Image>().sprite = null;

        foreach (var item in PlayerInventory)
        {
            UIInventory.Slots[i].GetChild(0).GetComponent<Image>().enabled = true;
            UIInventory.Slots[i].GetChild(0).GetComponent<Image>().sprite = item.ItemIcon;
            i++;
        }
    }

    public void DoIfActive()
    {
        if (Input.GetKeyDown(KeyCode.X))
            DestroyItem();

        if (Input.GetKeyDown(KeyCode.P))
            if (SelectedItem != null)
                if (isInEq)
                    UnequipItem(EquipmentSlots[(int)SelectedItem.Slot]);
                else UseItemChoice();

        if (Input.GetKeyDown(KeyCode.O))
        {
            if (SelectedItem == null || (ShownItem == SelectedItem && ItemStatisticsObject.gameObject.activeSelf))
            {
                ItemStatisticsObject.gameObject.SetActive(false);
            }
            else
            {
                ItemStatisticsObject.gameObject.SetActive(true);
                ShowStatistics();
            }
        }

        if (isInEq)
            EquipmentSelectionMovement();
        else
        {
            InventorySelectionMovement();
            if (!isInEq)
                SelectNewItem();
        }
    }

    private void InventorySelectionMovement()
    {
        foreach (var ControlScheme in GameManager.instance.ControlSchemes)
        {
            if (MoveSelectionX(ControlScheme.Left, -1)) return;
            if (MoveSelectionX(ControlScheme.Right, 1)) return;
            if (MoveSelectionY(ControlScheme.Up, 1)) return;
            if (MoveSelectionY(ControlScheme.Down, -1)) return;
        }
    }

    private void EquipmentSelectionMovement()
    {
        foreach (var ControlScheme in GameManager.instance.ControlSchemes)
        {
            if (MoveSelectionEquipment(ControlScheme.Left, -1)) return;
            if (MoveSelectionEquipment(ControlScheme.Right, 1)) return;
        }
    }

    bool MoveSelectionEquipment(KeyCode kc, int i)
    {
        if (Input.GetKeyDown(kc))
        {
            SelectionIndex += i;
            
            if (SelectionIndex == 4) SelectionIndex--;

            if (SelectionIndex == -1)
            {
                SelectionIndex = LastSelectionIndex;

                if (SelectionIndex + 1 <= PlayerInventory.Count)
                    SelectedItem = PlayerInventory[SelectionIndex];
                else SelectedItem = null;

                isInEq = false;
                UIInventory.SelectedItemImage.transform.position = LastSelectionPosition;
            }
            else
                MoveSelectionEquipment(EquipmentSlots[SelectionIndex]);

            return true;
        }
        return false;
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
                    isInEq = true;
                    SelectionIndex = 0;
                    MoveSelectionEquipment(kc, 0);
                    return true;
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

    public void UseItemChoice()
    {  
        if (!SelectedItem.Consumable)
            EquipItem(EquipmentSlots[(int)SelectedItem.Slot]);
        else
        {
            EquipItemStats(SelectedItem);
            DestroyItem();
        }
    }


    private void UnequipItem(ItemEquipmentSlot lh)
    {
        if (!CheckIfFull())
        {
            UnequipItemStats(SelectedItem);
            PlayerInventory.Add(SelectedItem);

            lh.GetComponent<Image>().sprite = MaskSprite;

            SelectedItem = null;
            lh.ItemProperties = null;

            RefreshInventory();
        }
    }

    private void EquipItem(ItemEquipmentSlot lh)
    {
        EquipItemStats(SelectedItem);

        if (lh.ItemProperties == null) // Sprawdzam czy jest coś założone
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
            UnequipItemStats(tem);
            lh.SetProperties(PlayerInventory[SelectionIndex]);
            PlayerInventory[SelectionIndex] = tem;
            SelectedItem = tem;
        }

        RefreshInventory();
    }

    private void ShowStatistics()
    {
        ShownItem = SelectedItem;
 
        ItemName.text = SelectedItem.Name;
        ItemDesc.text = "\"" + SelectedItem.ItemDesc + "\"";

        if (SelectedItem.ShowStats)
            ItemStats.text = SelectedItem.GetStats();
        else ItemStats.text = "     ???";
    }

    private void EquipItemStats(Item i)
    {
        foreach (var v in i.Stats)
            switch (v.stat)
            {
                case StatType.Armor:
                    myPlayer.CurrentArmor += v.value;
                    break;
                case StatType.Damage:
                    myPlayer.CurrentDamage += v.value;
                    break;
                case StatType.Heal:
                    myPlayer.CurrentHP += v.value;
                    break;
            }
    }

    private void UnequipItemStats(Item i)
    {
        foreach (var v in i.Stats)
            switch (v.stat)
            {
                case StatType.Armor:
                    myPlayer.CurrentArmor -= v.value;
                    break;
                case StatType.Damage:
                    myPlayer.CurrentDamage -= v.value;
                    break;
            }
    }

    private void DestroyItem()
    {
        if (!isInEq)
        {
            PlayerInventory.RemoveAt(SelectionIndex);
            SelectedItem = null;
            RefreshInventory();
        }
    }

    public bool CheckIfFull()
    {
        if (UIInventory.Slots.Count <= PlayerInventory.Count)
            return true;
        return false;
    }
}
