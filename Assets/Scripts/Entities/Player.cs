using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(PlayerController))]
public class Player : Entity
{
    [SerializeField]
    InventoryManager inventoryManager;
    [SerializeField] Image eButton;
    [SerializeField] TextMeshProUGUI ePromptText;
    PlayerController playerController;
    Building nearBuilding;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
    }

    public void PlayerInteractWith(Entity entity)
    {
        if (entity == this)
            return;

        entity.ModifyHealth(-damage);
    }

    public void OpenCraftingMenu()
    {
        playerController.ToggleInventory();
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
            if(inventoryManager.TryAddToInventory(collision.GetComponent<DroppedItem>().item))
            {
                collision.transform.parent = transform;
                collision.transform.localPosition = new Vector2(0.67f, 0.87f);
                collision.gameObject.SetActive(false);
                
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
}
