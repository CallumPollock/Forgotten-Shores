using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class DroppedItem : FollowTarget
{
    public Item item;
    public Item itemInstance;
    public int stackSize = 1;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        if(itemInstance == null && item != null)
        {
            itemInstance = Object.Instantiate(item);
        }

        spriteRenderer = GetComponent<SpriteRenderer>();

    }

    public void UpdateSprite(Sprite sprite)
    {
        spriteRenderer.sprite = sprite;
    }
}
