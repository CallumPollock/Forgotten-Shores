using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Objective", menuName = "Custom/Objective")]
public class Objective : ScriptableObject
{
    public string objectiveID = System.Guid.NewGuid().ToString();

    public string description;

    public enum ObjectiveType
    {
        None, CraftItem, EntityDroppedItem, CompleteDialogue
    }

    public ObjectiveType type;
    public Item item;
    public int amountNeeded;
    public GameObject gameObject;

    public Objective[] nextObjective;

    public Item[] unlocksRecipes;
}
