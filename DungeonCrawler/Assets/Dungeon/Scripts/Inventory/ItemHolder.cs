using UnityEngine;
using System.Collections;

public class ItemHolder : MonoBehaviour
{
    // Use this for initialization
    public Item ItemProperties;
    private SpriteRenderer mySpriteRenderer;

    void Start()
    {
        mySpriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetProperties(Item i)
    {
        if (mySpriteRenderer == null)
            mySpriteRenderer = GetComponent<SpriteRenderer>();

        ItemProperties = i;
        mySpriteRenderer.sprite = ItemProperties.ItemIcon;
    }
}