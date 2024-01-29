using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class Entity : MonoBehaviour
{
    public int health;
    public int maxHealth;
    public int damage;
    //GameObject dmgIndicatorPrefab;
    //public DamageIndicator damageIndicator;

    public Sprite deathSprite;

    public GameObject resource;
    public float dropChance;

    //When the mouse hovers over the GameObject, it turns to this color (red)
    Color m_MouseOverColor = new Color(0.5f, 0.5f, 0.5f);

    //This stores the GameObjectís original color
    Color m_OriginalColor = new Color(1f, 1f, 1f);

    //Get the GameObjectís mesh renderer to access the GameObjectís material and color
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
        if (health <= 0)
            return;

        health = Mathf.Clamp(health + val, 0, maxHealth);

        StartCoroutine(CreateDamageIndicator(val));

        if (health <= 0)
        {
            OnDeath();
        }


        if (val < 0 && resource != null)
            if (Random.value <= dropChance)
                DropResource();
    }

    IEnumerator CreateDamageIndicator(int damage)
    {
        DamageIndicator newDmgIndicator = Instantiate(GameState.instance.damageIndicator).GetComponent<DamageIndicator>();
        newDmgIndicator.SetDamageValue(damage);
        Vector2 randomPos = Random.insideUnitCircle * 0.3f;
        newDmgIndicator.transform.position = new Vector3(transform.position.x+randomPos.x, transform.position.y+randomPos.y, -6f);

        newDmgIndicator.GetComponent<Rigidbody2D>().AddForce(Vector2.up*30f);

        yield return new WaitForSeconds(0.3f);
        Destroy(newDmgIndicator.gameObject);

    }


    public virtual void OnDeath()
    {
        if (deathSprite != null)
            spriteRenderer.sprite = deathSprite;
    }

    void DropResource()
    {
        GameObject resourceDrop = Instantiate(resource);
        resourceDrop.transform.position = new Vector2(transform.position.x, transform.position.y) + Random.insideUnitCircle * 0.8f;
    }

     void OnMouseOver()
    {
        // Change the color of the GameObject to red when the mouse is over GameObject
        spriteRenderer.color = m_MouseOverColor;
    }

    void OnMouseExit()
    {
        // Reset the color of the GameObject back to normal
        spriteRenderer.color = m_OriginalColor;
    }
}
