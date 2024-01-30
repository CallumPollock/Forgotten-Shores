using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public List<InventorySlot> slots = new List<InventorySlot>();

    private void Awake()
    {
        foreach(InventorySlot slot in transform.GetComponentsInChildren<InventorySlot>())
        {
            slots.Add(slot);
        }
    }

    public bool TryAddToInventory(Item item)
    {
        foreach(InventorySlot slot in slots)
        {
            if (slot.item == item)
            {
                slot.IncreaseStackAmount(1);
                return true;
 
            }
            else if (slot.GetStackCount() == 0)
            {
                slot.FillSlot(item);
                return true;
            }
        }
        return false;
    }
}
