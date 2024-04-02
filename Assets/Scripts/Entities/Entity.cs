using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
[SerializeField]
public class EntityData
{
    public string name;
    public int health;
    public int maxHealth;
    public int damage;
    public int defence;
    public float speed;

    public Transform transform;

    public List<Item> inventory = new List<Item>();
}

public abstract class Entity : MonoBehaviour
{
  
    public EntityData data = new EntityData();

    SpriteRenderer spriteRenderer;
    public Sprite deathSprite;

    
    //[SerializeField] private List<Item> startingItems = new List<Item>();
    public EventHandler<List<Item>> InventoryChanged;
    public EventHandler<Item> OnAddItem, OnRemoveItem;
    public float dropChance;

    public Action OnEntityDied;

    [SerializeField] Item requiredItem;

    public List<Item> GetInventory() { return data.inventory; }

    public static event Action<Item> OnEntityDropItem;

    public virtual void Start()
    {
        data.transform = transform;

        for (int i = 0; i < data.inventory.Count; i++)
        {
            Item newItem = Instantiate(data.inventory[i]);
            newItem.name = data.inventory[i].name;
            data.inventory[i] = newItem;
        }
    }

    public virtual void ModifyHealth(int val)
    {

        if (val < 0)
            val = Mathf.Min(val + data.defence, 0);

        data.health = Mathf.Clamp(data.health + val, 0, data.maxHealth);

        CreateDamageIndicator(val);

        if (data.health <= 0)
        {
            OnDeath();
        }


        if (val < 0 && data.inventory.Count > 0)
            if (UnityEngine.Random.value/-val <= dropChance)
                DropItem(data.inventory[UnityEngine.Random.Range(0, data.inventory.Count - 1)], new Vector2(transform.position.x, transform.position.y) + UnityEngine.Random.insideUnitCircle * 3f, Vector2.zero, 0);
    }

    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<DroppedItem>())
        {
            if (collision.GetComponent<DroppedItem>().item != null)
            {
                DroppedItem droppedItem = collision.GetComponent<DroppedItem>();
                if (droppedItem.GetVelocity().magnitude >= 0.2f)
                {
                    ModifyHealth(-droppedItem.item.damage);
                }
                else if(droppedItem.GetVelocity().magnitude <= 0.1f)
                {
                    if (AddToInventory(droppedItem.item))
                    {
                        Destroy(collision.gameObject);
                    }
                }


            }

        }
    }

    public virtual bool AddToInventory(Item newItem)
    {
        return false;
    }

    public void DestroyItemInInventory(Item item, int amount)
    {
        Item itemToBeRemoved = GetItemFromInventory(item.itemID);
        itemToBeRemoved.stack -= amount;

        if (itemToBeRemoved.stack <= 0)
        {
            RemoveItem(itemToBeRemoved);
            Destroy(itemToBeRemoved);
        }
    }

    public Item GetItemFromInventory(string itemID)
    {
        return data.inventory.Find(i => i.itemID == itemID);
    }

    void CreateDamageIndicator(int val)
    {
        if (val == 0) return;

        Color newColor = new Color();

        if (val < 0)
            newColor = Color.red;
        else if (val > 0)
            newColor = Color.green;

        CreateInfoText(Mathf.Abs(val).ToString(), newColor, 8f, 0.3f);
    }

    public void CreateInfoText(string text, Color color, float textSize, float lifespan)
    {
        DamageIndicator newDmgIndicator = Instantiate(GameState.instance.damageIndicator).GetComponent<DamageIndicator>();
        newDmgIndicator.SetText(text, color, textSize, lifespan);
        newDmgIndicator.SetColour(color);
        Vector2 randomPos = UnityEngine.Random.insideUnitCircle * 2f;
        newDmgIndicator.transform.position = new Vector3(transform.position.x+randomPos.x, transform.position.y+randomPos.y+1.5f, -6f);
        newDmgIndicator.transform.SetParent(transform);

        newDmgIndicator.GetComponent<Rigidbody2D>().AddForce(Vector2.up*60f);

    }

    public void AttackEntity(Entity entity, Item item)
    {
        if (entity == this)
            return;

        if (entity.requiredItem != null)
            if (entity.requiredItem.itemID != item.itemID)
                return;

        if(item == null)
            entity.ModifyHealth(-data.damage);
        else
            entity.ModifyHealth(-(data.damage +item.damage));
    }

    public Entity GetBestTargetEntity(List<Entity> entitiesInSight)
    {
        foreach(Entity entity in entitiesInSight)
        {
            if(entity != this)
            {
                return entity;
            }
        }
        return null;
    }


    public virtual void OnDeath()
    {
        OnEntityDied?.Invoke();

        foreach(Item item in data.inventory.ToArray())
        {
            DropItem(item, transform.position, Vector2.zero, 0);
        }

        DropItem(Instantiate(GameState.instance.experienceGem), transform.position, Vector2.zero, 0);
        data.defence = 100;

        spriteRenderer = GetComponent<SpriteRenderer>();
        if (deathSprite != null && spriteRenderer != null)
            spriteRenderer.sprite = deathSprite;
        else
            Destroy(gameObject);
    }

    public virtual void RemoveItem(Item item)
    {
        data.inventory.Remove(item);
        InventoryChanged?.Invoke(this, data.inventory);
    }    

    public void DropItem(Item item, Vector2 startPos, Vector2 direction, float velocity)
    {
        if (item == null) return;

        Item droppedItem;

        if (item.stack > 1)
        {
            droppedItem = Instantiate(item);
            droppedItem.stack = 1;
            item.stack--;
        }
        else
        {
            droppedItem = item;
            RemoveItem(item);
        }

        GameObject newDroppedObject = new GameObject();
        newDroppedObject.name = item.name;
        newDroppedObject.AddComponent<DroppedItem>().SetAsNewItem(droppedItem);
        newDroppedObject.AddComponent<PolygonCollider2D>().isTrigger = true;
        

        newDroppedObject.transform.position = startPos;
        newDroppedObject.transform.rotation = Quaternion.Euler(0f, 0f, UnityEngine.Random.Range(0f, 360f));

        Rigidbody2D droppedItemRB = newDroppedObject.AddComponent<Rigidbody2D>();
        droppedItemRB.gravityScale = 0f;
        droppedItemRB.velocity = direction * velocity;
        droppedItemRB.drag = 5f;

        

        if (item.stack <= 0) RemoveItem(item);

        OnEntityDropItem?.Invoke(item);
        InventoryChanged?.Invoke(this, data.inventory);
    }
}
