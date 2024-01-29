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

    public InventorySlot FindEmptyInventorySlot()
    {
        foreach(InventorySlot slot in slots)
        {
            if (slot.GetStackCount() == 0)
                return slot;
        }
        return null;
    }
}
