using System.Collections;
using System;
using UnityEngine;

[CreateAssetMenu(fileName ="New Item", menuName ="Custom/Item")]
public class Item : ScriptableObject
{
    public string description;
    public Sprite icon;
    public GameObject droppedObject;

    public int stack;
    public int maxStack = 1;

    public enum ItemType { normal, placeable, spear}
    public ItemType itemType;

    public string itemID = System.Guid.NewGuid().ToString();

    //public Item[] recipe;

    [Serializable]
    public struct Recipe
    {
        public Item ingredient;
        public int amount;
    }

    public Recipe[] recipe;
}
