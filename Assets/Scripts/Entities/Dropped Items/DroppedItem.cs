using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppedItem : Entity
{
    public Item item;

    private void Start()
    {
        defence = 1000;
        maxHealth = 1000;
        health = 1000;
    }
}
