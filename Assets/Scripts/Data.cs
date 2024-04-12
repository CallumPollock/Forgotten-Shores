using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

[Serializable]
public abstract class Data
{
    public string name;
    public int health;
    public int maxHealth;

    public Color color = Color.white;

    public int damage;

    [Header("Crafting Properties")]
    public string interactionMethod;
    public bool craftsExclusively;
    //public bool craftsOtherItems;

    public void Interact(Player player, string interaction)
    {
        MethodInfo mInfo = player.GetType().GetMethod(interaction);
        mInfo.Invoke(player, new object[] { this });
    }
}
