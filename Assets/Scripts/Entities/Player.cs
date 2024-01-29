using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{

    private void Start()
    {
        damage = 1;
    }

    public void PlayerInteractWith(Entity entity)
    {
        if (entity == this)
            return;

        entity.ModifyHealth(-damage);
    }
}
