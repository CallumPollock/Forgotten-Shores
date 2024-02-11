using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipmentSlot : InventorySlot
{
    public override void OnDrop(PointerEventData eventData)
    {
        base.OnDrop(eventData);
        //GameState.instance.player.EquipHat(GetComponentInChildren<DraggableItem>());
    }
}
