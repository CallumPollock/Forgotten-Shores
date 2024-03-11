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

    public bool isPickupable = false;

    Rigidbody2D rigidbody;

    private void Start()
    {
        if(item != null)
        {
            if (item.GetInstanceID() >= 0)
            {
                item = Instantiate(item);
            }
        }
        

        rigidbody = GetComponent<Rigidbody2D>();

    }

    private void Update()
    {
        if(rigidbody == null)
        {
            rigidbody = GetComponent<Rigidbody2D>();
            return;
        }

        if(rigidbody.velocity.magnitude == 0f)
        {
            isPickupable = true;
        }
            
    }

    public void SetAsNewItem(Item newItem)
    {
        item = newItem;
        if(item.name.EndsWith("(Clone)"))
            item.name = item.name.Substring(0, item.name.Length - 7);

        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer != null) 
            spriteRenderer.sprite = item.icon;
    }
}
