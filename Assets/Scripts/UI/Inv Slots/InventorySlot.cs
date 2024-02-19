using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour
{
    [SerializeField]Image icon;
    [SerializeField] GameObject itemDraggable;

    public DraggableItem GetDraggable()
    {
        if (GetComponentInChildren<DraggableItem>(true))
            return GetComponentInChildren<DraggableItem>();
        else return null;
    }
    public void CreateItemInSlot(Item item)
    {
        DraggableItem newDraggableItem = Instantiate(itemDraggable).GetComponent<DraggableItem>();
        newDraggableItem.SetItem(item);
        newDraggableItem.transform.SetParent(transform, false);
    }


    public virtual void OnItemRemovedFromSlot() { }

}
