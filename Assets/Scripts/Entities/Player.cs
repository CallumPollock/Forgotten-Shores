using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class Player : Humanoid
{
    [SerializeField] Image eButton;
    [SerializeField] TextMeshProUGUI ePromptText;
    [SerializeField] TextMeshProUGUI levelUI, experienceUI;
    BuildingEntity nearBuilding;
    int level, experience, experienceToNextLevel;
    [SerializeField] SpriteRenderer headgear;

    

    public void OpenCraftingMenu()
    {
        //playerController.ToggleInventory();
    }

    public void IncreaseExp(int amount)
    {
        experience += amount;

        if (experience >= experienceToNextLevel)
        {
            SetLevel(level + 1);
        }
        experienceUI.text = experience + "/" + experienceToNextLevel;
    }

    private void SetLevel(int value)
    {
        level = value;
        experience = experience - experienceToNextLevel;
        experienceToNextLevel = (int)(50f * (Mathf.Pow(level + 1, 2) - (5 * (level + 1)) + 8));
        levelUI.text = level.ToString();
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
            eButton.gameObject.SetActive(true);
            ePromptText.text = collision.name;
            nearBuilding = collision.GetComponent<BuildingEntity>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<BuildingEntity>())
        {

            eButton.gameObject.SetActive(false);
            nearBuilding = null;
        }
    }

    public void ThrowEquipped(int handIndex)
    {
        if (GetHands()[handIndex].GetEquippedItem() != null)
        {
            DropItem(GetHands()[handIndex].GetEquippedItem());
            GetHands()[handIndex].SetEquippedItem(null);
        }
    }

    public void EquipHat(Sprite hat)
    {
        headgear.sprite = hat;
    }
}
