using System.Collections;
using System;
using UnityEngine;
using OdinSerializer;


[Serializable]
public class ItemData : Data
{
    
    public int stack;

    public string description;
    
    public Item.ItemType itemType;

    public string pickupSoundName;

}

[CreateAssetMenu(fileName ="New Item", menuName ="Items/Base Item")]
[Serializable]
public class Item : SerializedScriptableObject
{
    public static Sprite GetItemIcon(ItemData item) { return Resources.Load<Sprite>("Icons/" + item.name); }
    public static AudioClip GetItemPickupSound(string audioName) { return Resources.Load<AudioClip>("Icons/" + audioName); }

    [SerializeField]
    public ItemData data;

    public enum ItemType { normal, spear, placeable, ranged }

    [Serializable]
    public struct Ingredient
    {
        public Item item;
        public int amount;
    }

    [Serializable]
    public struct Recipe
    {
        public Ingredient[] ingredients;
        public Item requiredBuilding;
    }

    public Recipe recipe;
}
