using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Workbench : Building
{
    public override void Interaction(Player player)
    {
        player.OpenCraftingMenu();
    }
}
