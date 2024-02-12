using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipmentSlot : InventorySlot
{
    [SerializeField] SpriteRenderer spriteRenderer;

    public override void OnDrop(PointerEventData eventData)
    {
        base.OnDrop(eventData);

        if (spriteRenderer != null && GetDraggable().GetItem() != null)
            spriteRenderer.sprite = GetDraggable().GetItem().icon;
    }

    public override void OnItemRemovedFromSlot()
    {
        spriteRenderer.sprite = null;
    }
}
