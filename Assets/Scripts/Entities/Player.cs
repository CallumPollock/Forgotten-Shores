using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.Tilemaps;

public class Player : Humanlike
{
    public static Action<BuildingEntity> PlayerEnterRangeOfBuilding;
    public static Action PlayerExitRangeOfBuilding;
    public static Action<Entity> OnPlayerDied;
    public static Action<Player> OnPlayerSpawn;

    public static Action<int, int> OnExpUp;
    public static Action<int> OnLevelUp;
    public static Action<List<ItemData>, int> PlayerInventoryChanged;

    BuildingEntity nearBuilding;
    [SerializeField] SpriteRenderer headgear;

    [SerializeField] Transform homePosition;
    private Vector3 oldPosition;

    [SerializeField] PlayerController playerController;

    public override void Start()
    {
        OnEntityDied += OnPlayerDied;
        OnEntityDied += _ => PlayerDied();
        InventoryChanged += PlayerInventoryChanged;
        CraftMenuManager.ItemCrafted += ItemCrafted;
        
        SaveLoadJSON.playerLoaded += LoadEntityData;
        OnPlayerSpawn?.Invoke(this);
        base.Start();
    }

    private void PlayerDied()
    {
        SaveLoadJSON.playerLoaded -= LoadEntityData;
        CraftMenuManager.ItemCrafted -= ItemCrafted;
    }

    public void ScrollEquippedItem(int handIndex, int scrollValue)
    {
        if (GetInventory().Count == 0)
            return;

        if (handIndex < GetHands().Count)
        {
            equippedIndex = Mathf.Clamp(GetInventory().IndexOf(GetHands()[handIndex].GetEquippedItem()) + scrollValue, 0, GetInventory().Count - 1);
            GetHands()[handIndex].SetEquippedItem(GetInventory()[equippedIndex]);

            InventoryChanged?.Invoke(GetInventory(), equippedIndex);
        }
    }

    private void ItemCrafted(Item _item)
    {
        foreach (Item.Ingredient ingredient in _item.recipe.ingredients)
        {
            DestroyItemInInventory(ingredient.item.data, ingredient.amount);
        }
    }

    public void OpenCraftingMenu(Data _data)
    {
        PlayerController.ToggleInventory?.Invoke(_data);
    }

    public void IncreaseExp(int amount)
    {
        data.experience += amount;

        if (data.experience >= data.experienceToNextLevel)
        {
            SetLevel(data.level + 1);
            CreateInfoText("Level Up!", Color.green, 10f, 1f);
            TriggerEntityInfo?.Invoke(this);
        }
        OnExpUp?.Invoke(data.experience, data.experienceToNextLevel);
    }

    private void SetLevel(int value)
    {
        data.level = value;
        data.experience = data.experience - data.experienceToNextLevel;
        data.experienceToNextLevel = (int)(50f * (Mathf.Pow(data.level + 1, 2) - (5 * (data.level + 1)) + 8));
        OnLevelUp?.Invoke(data.level);
    }

    public void Interact()
    {
        if (nearBuilding != null)
            nearBuilding.Interaction(this);
    }

    public override bool AddToInventory(DroppedItem newItem)
    {
        if(newItem.name.Contains("EXP"))
        {
            IncreaseExp(newItem.item.stack);
            return true;
        }

        return base.AddToInventory(newItem);
    }

    public override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);

        if (collision.GetComponent<BuildingEntity>())
        {
            PlayerEnterRangeOfBuilding?.Invoke(collision.GetComponent<BuildingEntity>());
            nearBuilding = collision.GetComponent<BuildingEntity>();
        }
    }

    public void EnterHome()
    {
        oldPosition = transform.position;
        transform.position = homePosition.position;
    }

    public void ExitHome()
    {
        transform.position = oldPosition;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<BuildingEntity>())
        {

            PlayerExitRangeOfBuilding?.Invoke();
            nearBuilding = null;
        }
    }

    public void ThrowEquipped(int handIndex)
    {
        if (GetHands()[handIndex].GetEquippedItem() != null)
        {
            DropItem(GetHands()[handIndex].GetEquippedItem(), GetHands()[handIndex].transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition) - GetHands()[handIndex].transform.position, 0.1f);
            //GetHands()[handIndex].SetEquippedItem(null);
        }
    }

    public void EquipHat(Sprite hat)
    {
        headgear.sprite = hat;
    }
}
