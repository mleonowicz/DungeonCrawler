using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ItemEquipmentSlot : MonoBehaviour
{
    private Image myImage;
    public Item ItemProperties;

    public void SetProperties(Item i)
    {
        myImage = GetComponent<Image>();

        ItemProperties = i;
        myImage.sprite = ItemProperties.ItemIcon;
    }
}
