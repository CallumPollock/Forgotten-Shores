using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sheep : Entity
{
    public override void OnAttacked(Entity entity)
    {
        
    }

    public override void Start()
    {
        data.maxHealth = 30;
        data.health = 30;
        dropChance = 0.1f;
        base.Start();
    }

}
