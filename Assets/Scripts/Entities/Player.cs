using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    [SerializeField]
    InventoryManager inventoryManager;

    public void PlayerInteractWith(Entity entity)
    {
        if (entity == this)
            return;

        entity.ModifyHealth(-damage);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.GetComponent<DroppedItem>())
        {
            if(inventoryManager.TryAddToInventory(collision.collider.GetComponent<DroppedItem>().item))
            {
                Destroy(collision.gameObject);
            }
            
        }
    }
}
