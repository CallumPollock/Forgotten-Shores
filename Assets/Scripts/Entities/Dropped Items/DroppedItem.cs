using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppedItem : FollowTarget
{
    public Item item;
    public Item itemInstance;
    public int stackSize = 1;


    private void Awake()
    {
        if(itemInstance == null && item != null)
        {
            itemInstance = Object.Instantiate(item);
        }
    }
}
