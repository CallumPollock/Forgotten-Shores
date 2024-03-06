using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

[CreateAssetMenu(fileName = "New Building Item", menuName = "Custom/Building Item")]
public class BuildingItem : Item
{
    public bool craftsOtherItems;
    public string interactionMethod;

    public void Interact(Player player, string interaction)
    {
        MethodInfo mInfo = player.GetType().GetMethod(interaction);
        mInfo.Invoke(player, null);
    }
}
