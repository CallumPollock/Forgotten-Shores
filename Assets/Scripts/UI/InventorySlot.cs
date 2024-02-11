using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    [SerializeField]Image icon;
    [SerializeField] GameObject itemDraggable;

    public Item GetItem()
    {
        if (GetComponentInChildren<DraggableItem>())
            return GetComponentInChildren<DraggableItem>().item;
        else return null;
    }
    public void CreateItemInSlot(Item item)
    {
        DraggableItem newDraggableItem = Instantiate(itemDraggable).GetComponent<DraggableItem>();
        newDraggableItem.SetItem(item);
        newDraggableItem.transform.SetParent(transform, false);
    }


    public virtual void OnDrop(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;
        DraggableItem draggableItem = dropped.GetComponent<DraggableItem>();
        if (transform.GetComponentInChildren<DraggableItem>())
        {
            transform.GetComponentInChildren<DraggableItem>().transform.SetParent(draggableItem.parentAfterDrag);
        }
        
        draggableItem.parentAfterDrag = transform;
    }
}
