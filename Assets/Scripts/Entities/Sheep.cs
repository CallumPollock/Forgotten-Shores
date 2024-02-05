using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sheep : Entity
{
    void Start()
    {
        maxHealth = 30;
        health = 30;
        dropChance = 0.1f;
    }

}
