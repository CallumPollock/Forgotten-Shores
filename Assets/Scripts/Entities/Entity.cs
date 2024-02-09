using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public abstract class Entity : MonoBehaviour
{
    [Header("Stats")]
    public int health;
    public int maxHealth;
    public int damage;
    public int defence;
    public float speed;
    

    public Sprite deathSprite;

    public List<GameObject> resource = new List<GameObject>();
    public float dropChance;
    

    [SerializeField] bool dropsExperience = true;
    //When the mouse hovers over the GameObject, it turns to this color (red)
    Color m_MouseOverColor = new Color(0.5f, 0.5f, 0.5f);

    //This stores the GameObject’s original color
    Color m_OriginalColor = new Color(1f, 1f, 1f);

    //Get the GameObject’s mesh renderer to access the GameObject’s material and color
    SpriteRenderer spriteRenderer;

    void Awake()
    {
        //Fetch the mesh renderer component from the GameObject
        spriteRenderer = GetComponent<SpriteRenderer>();
        //Fetch the original color of the GameObject

        //damageIndicator = Instantiate(dmgIndicatorPrefab).GetComponent<DamageIndicator>();
        //damageIndicator.name = this.name + ".dmgIndicator";
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


        if (val < 0 && resource.Count >= 0)
            if (Random.value <= dropChance)
                DropResource();
    }

    void CreateDamageIndicator(int damage)
    {
        DamageIndicator newDmgIndicator = Instantiate(GameState.instance.damageIndicator).GetComponent<DamageIndicator>();
        newDmgIndicator.SetDamageValue(damage);
        Vector2 randomPos = Random.insideUnitCircle * 0.3f;
        newDmgIndicator.transform.position = new Vector3(transform.position.x+randomPos.x, transform.position.y+randomPos.y+0.3f, -6f);

        newDmgIndicator.GetComponent<Rigidbody2D>().AddForce(Vector2.up*30f);

    }


    public virtual void OnDeath()
    {
        Instantiate(GameState.instance.experienceGem);
        defence = 100;
        if (deathSprite != null)
            spriteRenderer.sprite = deathSprite;
        else
            Destroy(gameObject);
    }

    void DropResource()
    {
        GameObject resourceDrop = Instantiate(resource[Random.Range(0,resource.Count)]);
        resourceDrop.transform.position = new Vector2(transform.position.x, transform.position.y) + Random.insideUnitCircle * 0.4f;
        resourceDrop.transform.rotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));
    }
}
