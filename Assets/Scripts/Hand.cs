using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{

    [SerializeField] Entity entity;
    Collider2D hitBox;

    [SerializeField] Vector2 offset;
    float handDirectionOffset;

    [SerializeField] GameObject equippedItem;
    Collider2D equippedItemCollider;
    [SerializeField] SpriteRenderer equippedItemSprite;
    bool isHitting = false;

    Item heldItem;

    private void Awake()
    {
        entity = GetComponentInParent<Entity>();
        hitBox = GetComponent<Collider2D>();
        equippedItemSprite = equippedItem.GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (isHitting)
        {

            if (Vector2.Distance(transform.position, new Vector2(transform.parent.position.x, transform.parent.position.y+offset.y)) < 2.5f)
            {
                if(handDirectionOffset == -90f)
                    transform.localPosition += transform.up * Time.deltaTime * 15f;
                else
                    transform.localPosition += transform.right * Time.deltaTime * 15f;
            }
            else
            {
                UpdateColliders(false);
                isHitting = false;
            }
                
        }
        else
            transform.position = Vector2.Lerp(transform.position, transform.parent.TransformPoint(offset), Time.deltaTime * 5f);
    }

    public Item GetHeldItem() { return heldItem; }
    public Transform GetEquippedItemTransform() { return equippedItem.transform; }
    public float GetHandDirectionOffset() { return handDirectionOffset; }
    

    public void EquipItemInHand(Item item)
    {
        heldItem = item;

        if (equippedItemSprite != null)
            equippedItemSprite.sprite = item.icon;

        if (equippedItemCollider != null)
            Destroy(equippedItemCollider);

        if(item.itemType == Item.ItemType.placeable)
        {
            equippedItemSprite.color = new Color(0.5f, 0.5f, 1f, 0.8f);
        }
        else
        {
            equippedItemCollider = equippedItem.AddComponent<BoxCollider2D>();
            equippedItemCollider.isTrigger = true;
            equippedItemCollider.enabled = false;
            equippedItemSprite.color = Color.white;
        }
        


        switch (item.itemType)
        {
            case Item.ItemType.normal:
                UpdateHandDirectionOffset(0f);
                break;
            case Item.ItemType.spear:
                UpdateHandDirectionOffset(-90f);
                break;
        }
    }

    public void UpdateHandDirectionOffset(float offset)
    {
        handDirectionOffset = offset;
    }

    public void Hit()
    {
        if (heldItem != null)
            if (heldItem.itemType == Item.ItemType.placeable) return;

        isHitting = true;
        UpdateColliders(true);
    }

    void UpdateColliders(bool state)
    {
        
        if (equippedItemCollider != null)
        {
            equippedItemCollider.enabled = state;
            hitBox.enabled = false;
        }
        else
            hitBox.enabled = state;

    }

    public void OnTriggerEnter2D(Collider2D collision)
    {

        if(collision.GetComponentInParent<Entity>())
        {
            entity.AttackEntity(collision.GetComponentInParent<Entity>());
        }    
        
    }
}
