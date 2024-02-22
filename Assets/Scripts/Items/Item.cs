using System.Collections;
using System;
using UnityEngine;

[CreateAssetMenu(fileName ="New Item", menuName ="Custom/Item")]
public class Item : ScriptableObject
{
    public string description;
    public Sprite icon;
    public Color color = Color.white;

    public int stack;


    public int damage;
    public enum ItemType { normal, placeable, spear}
    public ItemType itemType;

    public string itemID = System.Guid.NewGuid().ToString();

    //public Item[] recipe;

    [Serializable]
    public struct Ingredient
    {
        public Item item;
        public int amount;
    }

    public Ingredient[] recipe;

    public bool advancedRecipe;
}
