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
        foreach (InventorySlot slot in transform.GetComponentsInChildren<InventorySlot>(true))
        {
            slots.Add(slot);
        }

    }

    public void ChangeEquippedItem(int x)
    {

        currentEquippedIndex = Mathf.Clamp(currentEquippedIndex + x, 0, 5);

        if (slots[currentEquippedIndex].GetDraggable() == null) return;
        if (slots[currentEquippedIndex].GetDraggable().GetItem() == null) return;

        playerEquippedSprite.sprite = slots[currentEquippedIndex].GetDraggable().GetItem().icon;

        switch (slots[currentEquippedIndex].GetDraggable().GetItem().itemType)
        {
            case Item.ItemType.normal:
                hand.UpdateHandDirectionOffset(0f);
                break;
            case Item.ItemType.spear:
                hand.UpdateHandDirectionOffset(-90f);
                break;
        }
    }

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

    public DraggableItem GetDraggableFromItem(Item itemToLookFor)
    {
        foreach(InventorySlot slot in slots)
        {
            if(slot.GetDraggable() != null)
                if (slot.GetDraggable().GetItem() != null)
                    if (slot.GetDraggable().GetItem().itemID == itemToLookFor.itemID) return slot.GetDraggable();
        }
        return null;
    }

    public InventorySlot FindCompatibleEmptySlot(Item item)
    {
        foreach(InventorySlot slot in slots)
        {
            if (slot.GetDraggable() == null) return slot;
        }
        return null;
    }

    public bool TryAddToInventory(Item newItem)
    {
       DraggableItem existingDraggableItem = GetDraggableFromItem(newItem);
        if (existingDraggableItem != null)
        {
            if(existingDraggableItem.GetItem().stack + newItem.stack > existingDraggableItem.GetItem().maxStack)
            {
                InventorySlot availableSlot = FindCompatibleEmptySlot(newItem);
                if (availableSlot != null)
                {
                    availableSlot.CreateItemInSlot(newItem);
                    return true;
                }
                else return false;
            }
            else
            {
                existingDraggableItem.GetItem().stack = Mathf.Min(existingDraggableItem.GetItem().maxStack, existingDraggableItem.GetItem().stack + newItem.stack);
                existingDraggableItem.UpdateStack();
                return true;
            }
            
            
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
