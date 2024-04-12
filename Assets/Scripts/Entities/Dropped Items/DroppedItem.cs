using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class DroppedItem : FollowTarget
{
    public ItemData item;
    //public Item itemInstance;
    private SpriteRenderer spriteRenderer;

    Rigidbody2D rb;

    private void Start()
    {
        if(item != null)
        {
            /*if (item.GetInstanceID() >= 0)
            {
                item = Instantiate(item);
            }*/
        }
        

        rb = GetComponent<Rigidbody2D>();

    }

    public Vector2 GetVelocity()
    {
        return rb.velocity;
    }

    private void Update()
    {
        if(rb == null)
        {
            rb = GetComponent<Rigidbody2D>();
            return;
        }
            
    }

    public void SetAsNewItem(ItemData newItem)
    {
        item = newItem;
        if(item.name.EndsWith("(Clone)"))
            item.name = item.name.Substring(0, item.name.Length - 7);

        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.material = GameState.instance.defaultLitSprite;

        if (spriteRenderer != null) 
            spriteRenderer.sprite = Item.GetItemIcon(item);
    }
}
