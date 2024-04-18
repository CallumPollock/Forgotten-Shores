using OdinSerializer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

[Serializable]
[SerializeField]
public class EntityData : Data
{    
    public int level, experience, experienceToNextLevel;

    public int defence;
    public float speed;

    public Vector2 entitySize;
    public float voicePitch;

    public Vector2 worldPosition;

    public List<ItemData> inventory = new List<ItemData>();
}

public abstract class Entity : MonoBehaviour
{
  
    public EntityData data = new EntityData();

    SpriteRenderer spriteRenderer;
    public Sprite deathSprite;
    [SerializeField] Transform body;
    [SerializeField] AudioClip[] idleSounds;
    [SerializeField] AudioClip[] hitSounds;
    private AudioSource audioSource;

    public int equippedIndex;
    
    //[SerializeField] private List<Item> startingItems = new List<Item>();
    public Action<List<ItemData>, int> InventoryChanged;
    public EventHandler<ItemData> OnAddItem, OnRemoveItem;
    public float dropChance;

    public Action<Entity> OnEntityDied;

    public Action<int, int> OnHealthModified;
    public static Action<Entity> TriggerEntityInfo;

    [SerializeField] Item[] requiredItem;
    Entity target;
    public List<ItemData> GetInventory() { return data.inventory; }

    public static event Action<ItemData> OnEntityDropItem;

    bool isDrowning;

    [SerializeField] SpriteRenderer[] hiddenInWater;

    private DamageFlash damageFlash;

    public virtual void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.spatialBlend = 1f;
        audioSource.maxDistance = 25f;
        audioSource.rolloffMode = AudioRolloffMode.Linear;
        audioSource.pitch = 2f-data.entitySize.x;
        if (idleSounds.Length > 0)
        {
            StartCoroutine(MakeSound());
        }

        damageFlash = gameObject.GetComponent<DamageFlash>();
        if(damageFlash == null)
            damageFlash = gameObject.AddComponent<DamageFlash>();
    }

    IEnumerator MakeSound()
    {
        audioSource.PlayOneShot(idleSounds[UnityEngine.Random.Range(0, idleSounds.Length)]);
        yield return new WaitForSeconds(UnityEngine.Random.Range(4f, 15f));
        StartCoroutine(MakeSound());
    }

    public void LoadEntityData(EntityData _data)
    {
        data = _data;
        transform.position = data.worldPosition;
        if(body != null )
        {
            body.localScale = _data.entitySize;
        }
    }

    public virtual void Update()
    {
        data.worldPosition = transform.position;

        if(GameState.instance.tilemap != null)
        {
            TileBase tile = GameState.instance.tilemap.GetTile(GameState.instance.tilemap.WorldToCell(Vector3Int.RoundToInt(transform.position)));
            if(tile != null)
            {
                if (tile.name == "Ocean")
                {
                    if (!isDrowning)
                    {
                        isDrowning = true;
                        StartCoroutine(EntityDrowning());
                        foreach (SpriteRenderer sr in hiddenInWater)
                        {
                            sr.enabled = false;
                        }
                    }

                }
                else
                {
                    isDrowning = false;
                    StopCoroutine(EntityDrowning());
                    foreach (SpriteRenderer sr in hiddenInWater)
                    {
                        sr.enabled = true;
                    }
                }
            }
            

        }
    }

    IEnumerator EntityDrowning()
    {
        yield return new WaitForSeconds(1f);
        ModifyHealth(-3);
        if(isDrowning) 
        { 
            StartCoroutine(EntityDrowning());
        }
    }

    public virtual void ModifyHealth(int val)
    {

        if (val < 0)
        {
            val = Mathf.Min(val + data.defence, 0);
            if(damageFlash != null)
                damageFlash.CallDamageFlash();
            if(hitSounds.Length > 0)
            {
                audioSource.PlayOneShot(hitSounds[UnityEngine.Random.Range(0, hitSounds.Length)]);
            }
        }
            

        data.health = Mathf.Clamp(data.health + val, 0, data.maxHealth);

        CreateDamageIndicator(val);

        if (data.health <= 0)
        {
            OnDeath();
        }
        else
        {
            TriggerEntityInfo?.Invoke(this);
        }

        OnHealthModified?.Invoke(data.health, data.maxHealth);


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
                if (droppedItem.GetVelocity().magnitude >= 0.3f)
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

    public virtual bool AddToInventory(ItemData newItem)
    {
        return false;
    }

    public void DestroyItemInInventory(ItemData item, int amount)
    {
        ItemData itemToBeRemoved = GetItemFromInventory(item.name);
            itemToBeRemoved.stack -= amount;

        if (itemToBeRemoved.stack <= 0)
        {
            RemoveItem(itemToBeRemoved);
            //Destroy(itemToBeRemoved);
        }
        InventoryChanged?.Invoke(data.inventory, equippedIndex);
    }

    public ItemData GetItemFromInventory(String itemName)
    {
        return data.inventory.Find(i => i.name == itemName);
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

    public abstract void OnAttacked(Entity entity);

    public void AttackEntity(Entity entity, ItemData item)
    {
        if (entity == this)
            return;

        if (entity.requiredItem.Length > 0)
        {
            if(entity.requiredItem.Where(i => i.name == item.name) == null)
            {
                return;
            }
        }

        if(item == null)
            entity.ModifyHealth(-data.damage);
        else
            entity.ModifyHealth(-(data.damage +item.damage));

        entity.OnAttacked(this);
    }

    public Entity GetBestTargetEntity(List<Entity> entitiesInSight)
    {
        Entity bestTarget = entitiesInSight.Where(e => e.GetType() == typeof(Player)).FirstOrDefault();
        if(bestTarget == null)
        {
            bestTarget = entitiesInSight.FirstOrDefault();
        }

        return bestTarget;
    }

    public void SetTarget(Entity _entity){ target = _entity; }
    public Entity GetTarget() { return target; }
    public virtual void OnDeath()
    {
        OnEntityDied?.Invoke(this);

        StopAllCoroutines();

        foreach(ItemData item in data.inventory.ToArray())
        {
            DropItem(item, transform.position, Vector2.zero, 0);
        }

        CreateDroppedItem(GameState.instance.experienceGem.data, transform.position, Vector2.zero, 0f);
        data.defence = 100;

        spriteRenderer = GetComponent<SpriteRenderer>();
        if (deathSprite != null && spriteRenderer != null)
            spriteRenderer.sprite = deathSprite;
        else
            gameObject.SetActive(false);
    }

    public void RespawnEntity()
    {
        gameObject.SetActive(true);
        data.health = data.maxHealth;
    }

    public virtual void RemoveItem(ItemData item)
    {
        data.inventory.Remove(item);
        InventoryChanged?.Invoke(data.inventory, equippedIndex);
    }    

    public void DropItem(ItemData item, Vector2 startPos, Vector2 direction, float velocity)
    {
        if (item == null) return;

        ItemData droppedItem = new ItemData();

        if (item.stack > 1)
        {
            droppedItem = item;
            droppedItem.stack = 1;
            item.stack--;
        }
        else
        {
            droppedItem = item;
            RemoveItem(item);
        }


        CreateDroppedItem(droppedItem, startPos, direction, velocity);
        

        if (item.stack <= 0) RemoveItem(item);

        OnEntityDropItem?.Invoke(item);
        InventoryChanged?.Invoke(data.inventory, equippedIndex);
    }

    private void CreateDroppedItem(ItemData droppedItem, Vector2 startPos, Vector2 direction, float velocity)
    {
        GameObject newDroppedObject = new GameObject();
        newDroppedObject.name = droppedItem.name;
        newDroppedObject.AddComponent<DroppedItem>().SetAsNewItem(droppedItem);
        newDroppedObject.AddComponent<PolygonCollider2D>().isTrigger = true;


        newDroppedObject.transform.position = startPos;
        newDroppedObject.transform.rotation = Quaternion.Euler(0f, 0f, UnityEngine.Random.Range(0f, 360f));

        Rigidbody2D droppedItemRB = newDroppedObject.AddComponent<Rigidbody2D>();
        droppedItemRB.gravityScale = 0f;
        droppedItemRB.velocity = direction * velocity;
        droppedItemRB.drag = 5f;
    }
}
