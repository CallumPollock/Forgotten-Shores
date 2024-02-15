using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Player : Entity
{
    [SerializeField]
    InventoryManager inventoryManager;
    [SerializeField] Image eButton;
    [SerializeField] TextMeshProUGUI ePromptText;
    [SerializeField] TextMeshProUGUI levelUI, experienceUI;
    Building nearBuilding;
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

    private void Update()
    {
        if (nearBuilding != null)
            if (Input.GetKeyDown(KeyCode.E))
                nearBuilding.Interaction(this);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<DroppedItem>())
        {
            if(collision.GetComponent<DroppedItem>().item != null)
            {
                if (inventoryManager.TryAddToInventory(collision.GetComponent<DroppedItem>().item))
                {
                    Destroy(collision.gameObject);

                }
            }
            else
            {
                IncreaseExp(10);
                Destroy(collision.gameObject);
            }

            
            
        }
        else if (collision.GetComponent<Building>())
        {

            eButton.gameObject.SetActive(true);
            ePromptText.text = collision.name;
            nearBuilding = collision.GetComponent<Building>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Building>())
        {

            eButton.gameObject.SetActive(false);
            nearBuilding = null;
        }
    }

    public void EquipHat(Sprite hat)
    {
        headgear.sprite = hat;
    }
}
