using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingEntity : Entity
{
    public void Interaction(Player player)
    {
        player.OpenCraftingMenu();
    }
}
