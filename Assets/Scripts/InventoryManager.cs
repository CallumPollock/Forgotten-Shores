using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private List<InventorySlot> slots = new List<InventorySlot>();
    int currentEquippedIndex;
    public SpriteRenderer playerEquippedSprite;
    public Hand hand;
    [SerializeField] GameObject droppedItem;


    private void Awake()
    {
        foreach (InventorySlot slot in transform.GetComponentsInChildren<InventorySlot>())
        {
            slots.Add(slot);
        }
    }

    /*public void ChangeEquippedItem(int x)
    {
        if (inventory.Count <= 0)
            return;

        currentEquippedIndex = Mathf.Clamp(currentEquippedIndex + x, 0, inventory.Count - 1);
        playerEquippedSprite.sprite = inventory[currentEquippedIndex].icon;

        switch (inventory[currentEquippedIndex].itemType)
        {
            case Item.ItemType.normal:
                hand.UpdateHandDirectionOffset(0f);
                break;
            case Item.ItemType.spear:
                hand.UpdateHandDirectionOffset(-90f);
                break;
        }
    }*/

    /*public void DropItem()
    {
        if (currentEquippedIndex > inventory.Count - 1)
            return;

        playerEquippedSprite.sprite = null;

        DroppedItem newDroppedItem = Instantiate(droppedItem).GetComponent<DroppedItem>();
        newDroppedItem.item = inventory[currentEquippedIndex];
        newDroppedItem.itemInstance = inventory[currentEquippedIndex];
        newDroppedItem.UpdateSprite(inventory[currentEquippedIndex].icon);

        newDroppedItem.transform.position = new Vector2(hand.transform.position.x, hand.transform.position.y) + Random.insideUnitCircle * 0.8f;
        newDroppedItem.transform.rotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));

        inventory.RemoveAt(currentEquippedIndex);
        slots[currentEquippedIndex].EmptySlot();
    }*/

    public Item GetItemFromInventory(Item itemToLookFor)
    {
        foreach(InventorySlot slot in slots)
        {
            if(slot.GetItem() != null)
                if (slot.GetItem().itemID == itemToLookFor.itemID) return slot.GetItem();
        }
        return null;
    }

    public InventorySlot FindCompatibleEmptySlot(Item item)
    {
        foreach(InventorySlot slot in slots)
        {
            if (slot.GetItem() == null) return slot;
        }
        return null;
    }

    public bool TryAddToInventory(Item newItem)
    {
       Item existingItem = GetItemFromInventory(newItem);
        if (existingItem != null)
        {
            existingItem.stack = Mathf.Min(existingItem.maxStack, existingItem.stack + newItem.stack);
            return true;
        }
        else
        {
            InventorySlot availableSlot = FindCompatibleEmptySlot(newItem);
            if (availableSlot != null)
            {
                availableSlot.CreateItemInSlot(newItem);
                return true;
            }
            else return false;

        }
    }
}
