using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : Entity
{
    // Start is called before the first frame update
    void Start()
    {
        maxHealth = 30;
        health = 30;
        dropChance = 0.1f;
    }

    
}
