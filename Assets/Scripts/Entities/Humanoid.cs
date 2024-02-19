using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Humanoid : Entity
{
    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<DroppedItem>())
        {
            if (collision.GetComponent<DroppedItem>().item != null)
            {
                AddToInventory(collision.GetComponent<DroppedItem>().item);
                Destroy(collision.gameObject);
            }

        }
    }
}
