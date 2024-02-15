using System.Collections;
using System.Collections.Generic;
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

    public List<Item> resource = new List<Item>();
    public float dropChance;
    

    [SerializeField] bool dropsExperience = true;

    [SerializeField] List<Hand> hands = new List<Hand>();

    public virtual void Awake()
    {
        foreach (Hand hand in GetComponentsInChildren<Hand>())
        {
            hands.Add(hand);
        }
    }

    public List<Hand> GetHands() { return hands; }

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


        if (val < 0 && resource.Count >= 0)
            if (Random.value <= dropChance)
                DropResource();
    }

    void CreateDamageIndicator(int damage)
    {
        DamageIndicator newDmgIndicator = Instantiate(GameState.instance.damageIndicator).GetComponent<DamageIndicator>();
        newDmgIndicator.SetDamageValue(damage);
        Vector2 randomPos = Random.insideUnitCircle * 2f;
        newDmgIndicator.transform.position = new Vector3(transform.position.x+randomPos.x, transform.position.y+randomPos.y+1.5f, -6f);

        newDmgIndicator.GetComponent<Rigidbody2D>().AddForce(Vector2.up*50f);

    }

    public void AttackEntity(Entity entity)
    {
        if (entity == this)
            return;

        entity.ModifyHealth(-damage);
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
        Instantiate(GameState.instance.experienceGem);
        defence = 100;

        spriteRenderer = GetComponent<SpriteRenderer>();
        if (deathSprite != null && spriteRenderer != null)
            spriteRenderer.sprite = deathSprite;
        else
            Destroy(gameObject);
    }

    void DropResource()
    {

        GameObject newDroppedObject = new GameObject();
        newDroppedObject.AddComponent<DroppedItem>().SetAsNewItem(resource[Random.Range(0, resource.Count - 1)]);
        newDroppedObject.AddComponent<PolygonCollider2D>().isTrigger = true;

        newDroppedObject.transform.position = new Vector2(transform.position.x, transform.position.y) + Random.insideUnitCircle * 0.4f;
        newDroppedObject.transform.rotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));
    }
}
