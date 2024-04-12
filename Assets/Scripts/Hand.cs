using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{

    [SerializeField] Humanlike entity;
    Collider2D hitBox;

    [SerializeField] Vector2 offset;
    float handDirectionOffset;

    [SerializeField] private GameObject equippedItemGO;
    [SerializeField] private ItemData equippedItem;
    Collider2D equippedItemCollider;

    [SerializeField] private SpriteRenderer equippedItemSprite;
    bool isHitting = false;

    [SerializeField] private Vector2 defaultItemPosition;
    [SerializeField] private Quaternion defaultItemRotation;

    private void Awake()
    {
        entity = GetComponentInParent<Humanlike>();
        hitBox = GetComponent<Collider2D>();
        equippedItemSprite = equippedItemGO.GetComponent<SpriteRenderer>();

        defaultItemPosition = equippedItemGO.transform.localPosition;
        defaultItemRotation = equippedItemGO.transform.localRotation;
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

    public ItemData GetEquippedItem() { return equippedItem; }
    public Transform GetEquippedItemTransform() { return equippedItemGO.transform; }
    public float GetHandDirectionOffset() { return handDirectionOffset; }
    
    public bool GetIsHitting() { return isHitting; }

    public void SetEquippedItem(ItemData item)
    {
        if(item == null)
        {
            equippedItem = null;
            equippedItemSprite.sprite = null;
            Destroy(equippedItemCollider);
            return;
        }

        equippedItem = item;

        if (equippedItemSprite != null)
            equippedItemSprite.sprite = Item.GetItemIcon(item);

        if (equippedItemCollider != null)
            Destroy(equippedItemCollider);

        if(item.itemType == Item.ItemType.placeable)
        {
            equippedItemSprite.color = new Color(0.5f, 0.5f, 1f, 0.8f);
        }
        else
        {
            equippedItemCollider = equippedItemGO.AddComponent<BoxCollider2D>();
            equippedItemCollider.isTrigger = true;
            equippedItemCollider.enabled = false;
            equippedItemSprite.color = Color.white;
            equippedItemGO.transform.localPosition = defaultItemPosition;
            equippedItemGO.transform.localRotation = defaultItemRotation;
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

    public void PlaceBuilding()
    {
        if (equippedItem == null) return;

        GameObject newBuilding = new GameObject();

        newBuilding.AddComponent<Workbench>().SetItem(equippedItem);
        newBuilding.AddComponent<SpriteRenderer>().sprite = Item.GetItemIcon(equippedItem);
        newBuilding.AddComponent<BoxCollider2D>().isTrigger = true;
        newBuilding.transform.position = equippedItemGO.transform.position;
        newBuilding.name = equippedItem.name;

        entity.GetInventory().Remove(equippedItem);
        SetEquippedItem(null);
        
    }

    public void Hit()
    {
        if (equippedItem != null)
        {
            switch (equippedItem.itemType)
            {
                case Item.ItemType.placeable:
                    PlaceBuilding();
                    return;
                case Item.ItemType.ranged:
                    entity.DropItem(entity.GetHands()[1].GetEquippedItem(), entity.GetHands()[1].transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition) - entity.GetHands()[1].transform.position, 7f);
                    return;
            }
        }
            

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
            entity.AttackEntity(collision.GetComponentInParent<Entity>(), equippedItem);
        }    
        
    }
}
