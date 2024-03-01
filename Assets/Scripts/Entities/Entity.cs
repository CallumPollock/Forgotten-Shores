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
    public float dropChance;

    public List<Item> GetInventory() { return inventory; }

    private void Start()
    {
        for (int i = 0; i < inventory.Count; i++)
        {
            inventory[i] = Instantiate(inventory[i]);
        }
    }

    public virtual void ModifyHealth(int val)
    {

        if (val < 0)
            val = Mathf.Min(val + defence, 0);

        health = Mathf.Clamp(health + val, 0, maxHealth);

        if(val != 0)
            CreateDamageIndicator(val);

        if (health <= 0)
        {
            OnDeath();
        }


        if (val < 0 && inventory.Count > 0)
            if (UnityEngine.Random.value/-val <= dropChance)
                DropItem(inventory[UnityEngine.Random.Range(0, inventory.Count - 1)]);
    }

    public virtual bool AddToInventory(Item newItem)
    {
        if (newItem.name.Contains("EXP")) return false;

        if (inventory.Any(i => i.itemID == newItem.itemID))
        {
            GetItemFromInventory(newItem.itemID).stack += newItem.stack;
        }
        else
        {
            inventory.Add(newItem);
        }
        InventoryChanged?.Invoke(this, inventory);

        DamageIndicator newDmgIndicator = Instantiate(GameState.instance.damageIndicator).GetComponent<DamageIndicator>();
        newDmgIndicator.SetText(String.Format("+{0} {1}" , newItem.stack, newItem.name));
        newDmgIndicator.transform.position = new Vector3(transform.position.x, transform.position.y + 1.5f, -6f);
        newDmgIndicator.GetComponent<Rigidbody2D>().AddForce(Vector2.up * 50f);

        return true;
    }

    public Item GetItemFromInventory(string itemID)
    {
        return inventory.Find(i => i.itemID == itemID);
    }

    void CreateDamageIndicator(int damage)
    {
        DamageIndicator newDmgIndicator = Instantiate(GameState.instance.damageIndicator).GetComponent<DamageIndicator>();
        newDmgIndicator.SetDamageValue(damage);
        Vector2 randomPos = UnityEngine.Random.insideUnitCircle * 2f;
        newDmgIndicator.transform.position = new Vector3(transform.position.x+randomPos.x, transform.position.y+randomPos.y+1.5f, -6f);

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
        foreach(Item item in inventory.ToArray())
        {
            DropItem(item);
        }

        DropItem(Instantiate(GameState.instance.experienceGem));
        defence = 100;

        spriteRenderer = GetComponent<SpriteRenderer>();
        if (deathSprite != null && spriteRenderer != null)
            spriteRenderer.sprite = deathSprite;
        else
            Destroy(gameObject);
    }

    public void DropItem(Item item)
    {
        GameObject newDroppedObject = new GameObject();
        newDroppedObject.name = item.name;
        newDroppedObject.AddComponent<DroppedItem>().SetAsNewItem(item);
        newDroppedObject.AddComponent<PolygonCollider2D>().isTrigger = true;

        newDroppedObject.transform.position = new Vector2(transform.position.x, transform.position.y) + UnityEngine.Random.insideUnitCircle * 3f;
        newDroppedObject.transform.rotation = Quaternion.Euler(0f, 0f, UnityEngine.Random.Range(0f, 360f));

        inventory.Remove(item);
    }
}
