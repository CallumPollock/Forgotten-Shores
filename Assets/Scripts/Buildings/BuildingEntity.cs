using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingEntity : Entity
{
    [SerializeField] BuildingItem buildingItem;
    public void Interaction(Player player)
    {
        buildingItem.Interact(player, buildingItem.interactionMethod);
    }

    public void SetItem(BuildingItem item)
    {
        buildingItem = item;
        AddToInventory(item);
    }
}
