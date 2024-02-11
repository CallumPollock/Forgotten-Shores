using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class DroppedItem : FollowTarget
{
    public Item item;
    //public Item itemInstance;
    public int stackSize = 1;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        if(item != null)
        {
            if (item.GetInstanceID() >= 0)
            {
                item = Instantiate(item);
            }
        }
        

        

    }

    public void SetAsNewItem(Item newItem)
    {
        item = Instantiate(newItem);

        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer != null) 
            spriteRenderer.sprite = item.icon;
    }
}
