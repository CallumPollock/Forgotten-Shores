using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class Player : Human
{
    public static Action<BuildingEntity> PlayerEnterRangeOfBuilding;
    public static Action PlayerExitRangeOfBuilding;
    public static Action OnPlayerDied;
    public static Action<Player> OnPlayerSpawn;

    public static Action<int, int> OnExpUp;
    public static Action<int> OnLevelUp;
    public static EventHandler<List<Item>> PlayerInventoryChanged;

    BuildingEntity nearBuilding;
    int level, experience, experienceToNextLevel;
    [SerializeField] SpriteRenderer headgear;

    [SerializeField] Transform homePosition;
    private Vector3 oldPosition;

    [SerializeField] PlayerController playerController;

    [SerializeField] Image scrollUI1, scrollUI2, scrollUI3;

    public override void Start()
    {
        base.Start();

        OnEntityDied += OnPlayerDied;
        InventoryChanged += PlayerInventoryChanged;
        CraftMenuManager.ItemCrafted += ItemCrafted;
        OnPlayerSpawn?.Invoke(this);
        data = SaveLoadJSON.worldData.player;
    }

    public void ScrollEquippedItem(int handIndex, int scrollValue)
    {
        if (GetInventory().Count == 0)
            return;

        if (handIndex < GetHands().Count)
        {
            int itemIndex = Mathf.Clamp(GetInventory().IndexOf(GetHands()[handIndex].GetEquippedItem()) + scrollValue, 0, GetInventory().Count - 1);
            GetHands()[handIndex].SetEquippedItem(GetInventory()[itemIndex]);
            scrollUI2.sprite = GetInventory()[itemIndex].icon;

            if (itemIndex - 1 > 0)
            {
                scrollUI1.sprite = GetInventory()[itemIndex - 1].icon;
                scrollUI1.gameObject.SetActive(true);
            }
            else
                scrollUI1.gameObject.SetActive(false);

            if (itemIndex + 1 < GetInventory().Count-1)
            {
                scrollUI3.sprite = GetInventory()[itemIndex + 1].icon;
                scrollUI3.gameObject.SetActive(true);
            }
            else
                scrollUI3.gameObject.SetActive(false);

        }
    }

    private void ItemCrafted(Item _item)
    {
        foreach (Item.Ingredient ingredient in _item.recipe.ingredients)
        {
            DestroyItemInInventory(ingredient.item, ingredient.amount);
        }
    }

    public void OpenCraftingMenu(BuildingItem buildingItem)
    {
        PlayerController.ToggleInventory?.Invoke(buildingItem);
    }

    public void IncreaseExp(int amount)
    {
        experience += amount;

        if (experience >= experienceToNextLevel)
        {
            SetLevel(level + 1);
            CreateInfoText("Level Up!", Color.green, 10f, 1f);
        }
        OnExpUp?.Invoke(experience, experienceToNextLevel);
    }

    private void SetLevel(int value)
    {
        level = value;
        experience = experience - experienceToNextLevel;
        experienceToNextLevel = (int)(50f * (Mathf.Pow(level + 1, 2) - (5 * (level + 1)) + 8));
        OnLevelUp?.Invoke(level);
    }

    public void Interact()
    {
        if (nearBuilding != null)
            nearBuilding.Interaction(this);
    }

    public override bool AddToInventory(Item newItem)
    {
        if(newItem.name.Contains("EXP"))
        {
            IncreaseExp(newItem.stack);
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
