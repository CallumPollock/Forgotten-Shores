using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    [Header("Stats")]
    public int health;
    public int maxHealth;
    public int damage;
    public int defence;
    public float speed;


    SpriteRenderer spriteRenderer;
    public Sprite deathSprite;

    [SerializeField] private List<Item> inventory = new List<Item>();
    //[SerializeField] private List<Item> startingItems = new List<Item>();
    public EventHandler<List<Item>> InventoryChanged;
    public EventHandler<Item> OnAddItem, OnRemoveItem;
    public float dropChance;

    public Action OnEntityDied;

    public List<Item> GetInventory() { return inventory; }

    public static event Action<Item> OnEntityDropItem;

    public virtual void Start()
    {
        for (int i = 0; i < inventory.Count; i++)
        {
            Item newItem = Instantiate(inventory[i]);
            newItem.name = inventory[i].name;
            inventory[i] = newItem;
        }
    }

    public virtual void ModifyHealth(int val)
    {

        if (val < 0)
            val = Mathf.Min(val + defence, 0);

        health = Mathf.Clamp(health + val, 0, maxHealth);

        CreateDamageIndicator(val);

        if (health <= 0)
        {
            OnDeath();
        }


        if (val < 0 && inventory.Count > 0)
            if (UnityEngine.Random.value/-val <= dropChance)
                DropItem(inventory[UnityEngine.Random.Range(0, inventory.Count - 1)], new Vector2(transform.position.x, transform.position.y) + UnityEngine.Random.insideUnitCircle * 3f, Vector2.zero, 0);
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

    public Item GetItemFromInventory(string itemID)
    {
        return inventory.Find(i => i.itemID == itemID);
    }

    void CreateDamageIndicator(int val)
    {
        if (val == 0) return;

        Color newColor = new Color();

        if (val < 0)
            newColor = Color.red;
        else if (val > 0)
            newColor = Color.green;

        CreateInfoText(Mathf.Abs(val).ToString(), newColor);
    }

    public void CreateInfoText(string text, Color color)
    {
        DamageIndicator newDmgIndicator = Instantiate(GameState.instance.damageIndicator).GetComponent<DamageIndicator>();
        newDmgIndicator.SetText(text);
        newDmgIndicator.SetColour(color);
        Vector2 randomPos = UnityEngine.Random.insideUnitCircle * 2f;
        newDmgIndicator.transform.position = new Vector3(transform.position.x+randomPos.x, transform.position.y+randomPos.y+1.5f, -6f);
        newDmgIndicator.transform.SetParent(transform);

        newDmgIndicator.GetComponent<Rigidbody2D>().AddForce(Vector2.up*50f);

    }

    public void AttackEntity(Entity entity, Item item)
    {
        if (entity == this)
            return;

        if(item == null)
            entity.ModifyHealth(-damage);
        else
            entity.ModifyHealth(-(damage+item.damage));
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

        foreach(Item item in inventory.ToArray())
        {
            DropItem(item, transform.position, Vector2.zero, 0);
        }

        DropItem(Instantiate(GameState.instance.experienceGem), transform.position, Vector2.zero, 0);
        defence = 100;

        spriteRenderer = GetComponent<SpriteRenderer>();
        if (deathSprite != null && spriteRenderer != null)
            spriteRenderer.sprite = deathSprite;
        else
            Destroy(gameObject);
    }

    public virtual void RemoveItem(Item item)
    {
        inventory.Remove(item);
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
        InventoryChanged?.Invoke(this, inventory);
    }
}
