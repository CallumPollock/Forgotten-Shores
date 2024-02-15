using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CraftMenuManager : MonoBehaviour
{
    [SerializeField] List<Item> recipeBook = new List<Item>();
    [SerializeField] GameObject recipeButton;
    [SerializeField] GameObject ingredientObject;
    [SerializeField] Transform viewport;

    [Header("Selection Preview")]
    [SerializeField] TextMeshProUGUI previewName;
    [SerializeField] TextMeshProUGUI previewDescription;
    [SerializeField] Image previewIcon;
    [SerializeField] Transform recipePreviewContainer;

    [SerializeField] InventoryManager InventoryManager;

    [SerializeField] Button craftButton;

    // Start is called before the first frame update
    void Start()
    {
        UpdateCraftMenu();
    }

    public void UpdateCraftMenu()
    {
        foreach(Transform child in viewport)
        {
            Destroy(child.gameObject);
        }

        foreach (Item recipe in recipeBook)
        {
            RecipeButton newRecipeButton = Instantiate(recipeButton).GetComponent<RecipeButton>();
            newRecipeButton.InitialiseRecipeButton(recipe);
            newRecipeButton.transform.SetParent(viewport.transform, false);
            newRecipeButton.GetComponent<Button>().onClick.AddListener(delegate { UpdatePreview(recipe); });

            foreach(Item.Ingredient ingredient in recipe.recipe)
            {
                GameObject newIngredient = Instantiate(ingredientObject);
                newIngredient.GetComponentInChildren<Image>().sprite = ingredient.item.icon;
                newIngredient.GetComponentInChildren<TextMeshProUGUI>().text = ingredient.item.name + " (" + ingredient.amount + ")";
                newIngredient.transform.SetParent(newRecipeButton.recipeContainer);
            }
        }
    }

    public bool CheckItemCraftable(Item itemToCraft)
    {
        foreach(Item.Ingredient ingredient in itemToCraft.recipe)
        {
            if (InventoryManager.GetDraggableFromItem(ingredient.item) == null) return false;
            if (InventoryManager.GetDraggableFromItem(ingredient.item).GetItem() == null) return false;
            if (InventoryManager.GetDraggableFromItem(ingredient.item).GetItem().stack < ingredient.amount) return false;
        }

        return true;
    }

    public void CraftItem(Item itemToCraft)
    {
        if (!CheckItemCraftable(itemToCraft)) return;


        GameObject newDroppedObject = new GameObject();
        newDroppedObject.AddComponent<DroppedItem>().SetAsNewItem(itemToCraft);
        newDroppedObject.AddComponent<PolygonCollider2D>().isTrigger = true;

        newDroppedObject.transform.position = new Vector2(GameState.instance.player.transform.position.x, GameState.instance.player.transform.position.y) + Random.insideUnitCircle * 0.4f;
        newDroppedObject.transform.rotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));
    }

    public void UpdatePreview(Item itemSelection)
    {
        previewName.text = itemSelection.name;
        previewDescription.text = itemSelection.description;
        previewIcon.sprite = itemSelection.icon;

        foreach(Transform child in recipePreviewContainer)
        {
            Destroy(child.gameObject);
        }

        foreach (Item.Ingredient ingredient in itemSelection.recipe)
        {
            GameObject newIngredient = Instantiate(ingredientObject);
            newIngredient.GetComponentInChildren<Image>().sprite = ingredient.item.icon;
            newIngredient.GetComponentInChildren<TextMeshProUGUI>().text = ingredient.item.name + " (" + ingredient.amount + ")";
            newIngredient.transform.SetParent(recipePreviewContainer);
        }

        craftButton.interactable = CheckItemCraftable(itemSelection);
        craftButton.onClick.AddListener(delegate { CraftItem(itemSelection); });

    }
}
