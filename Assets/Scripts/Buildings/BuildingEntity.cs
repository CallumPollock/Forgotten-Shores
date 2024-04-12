using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingEntity : Entity
{
    public void Interaction(Player player)
    {
        data.Interact(player, data.interactionMethod);
    }

    public void SetItem(Data _data)
    {
        data.name = _data.name;
        data.color = _data.color;
        data.interactionMethod = _data.interactionMethod;
        data.craftsExclusively = _data.craftsExclusively;
        //AddToInventory(_data as ItemData);
    }
}
