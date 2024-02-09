using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public List<Item> inventory = new List<Item>();
    [SerializeField]private List<InventorySlot> slots = new List<InventorySlot>();
    int currentEquippedIndex;
    public SpriteRenderer playerEquippedSprite;


    private void Awake()
    {
        foreach(InventorySlot slot in transform.GetComponentsInChildren<InventorySlot>())
        {
            slots.Add(slot);
        }
    }

    public void ChangeEquippedItem(int x)
    {
        if (inventory.Count <= 0)
            return;

        currentEquippedIndex = Mathf.Clamp(currentEquippedIndex + x, 0, inventory.Count-1);
        playerEquippedSprite.sprite = inventory[currentEquippedIndex].icon;
    }

    public bool TryAddToInventory(Item newItem)
    {
        foreach(Item invItem in inventory)
        {
            if (invItem.itemID == newItem.itemID)
            {
                invItem.stack = Mathf.Min(invItem.maxStack, invItem.stack + newItem.stack);
                slots[inventory.IndexOf(invItem)].UpdateStack(invItem.stack);
                return true;
            }
            
        }
        if (inventory.Count < slots.Count)
        {
            slots[inventory.Count].FillSlot(newItem.icon, newItem.stack);
            inventory.Add(Object.Instantiate(newItem));
            return true;
        }
        else
            return false;
    }
}
