using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    [SerializeField]
    InventoryManager inventoryManager;

    private void Start()
    {
        damage = 1;
    }

    public void PlayerInteractWith(Entity entity)
    {
        if (entity == this)
            return;

        entity.ModifyHealth(-damage);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.GetComponent<Item>())
        {
            if(inventoryManager.FindEmptyInventorySlot() != null)
            {
                inventoryManager.FindEmptyInventorySlot().FillSlot(collision.collider.GetComponent<Item>());
                Destroy(collision.gameObject);
            }
            
        }
    }
}
